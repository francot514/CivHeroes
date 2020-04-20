#include "config.h"
#include "game.h"
#include "image_manager.h"
#include "data_manager.h"
#include "deck_manager.h"
#include "replay.h"
#include "materials.h"
#include "duelclient.h"
#include "netserver.h"
#include "single_mode.h"
#include <sstream>


#ifndef WIN32
#include <sys/types.h>
#include <dirent.h>
#endif

const unsigned short PRO_VERSION = 0x1333;

namespace ygo {

Game* mainGame;

bool Game::Initialize() {
	
	u32 width = 1024;
	u32 height = 640;

	srand(time(0));
	LoadConfig();
	LoadProgress();
	
	irr::SIrrlichtCreationParameters params = irr::SIrrlichtCreationParameters();
	params.AntiAlias = gameConf.antialias;
	if(gameConf.use_d3d)
		params.DriverType = irr::video::EDT_DIRECT3D9;
	else
		params.DriverType = irr::video::EDT_OPENGL;

	switch (gameConf.resolution)
	{

		case 1:

			width = 800;
			height = 600;

			break;


		case 2:

			width = 1024;
			height = 768;

			break;
		
	}
	params.WindowSize = irr::core::dimension2d<u32>(width, height);
	window_size = params.WindowSize;
	device = irr::createDeviceEx(params);
	if(!device)
		return false;

	// Apply skin
	unsigned int special_color = 0xFFFFFFFF;
	if (gameConf.skin_index > 0)
	{
		skinSystem = new CGUISkinSystem("skins", device);
		core::array<core::stringw> skins = skinSystem->listSkins();
		if ((size_t)gameConf.skin_index < skins.size())
		{
			int index = skins.size() - gameConf.skin_index; // reverse index
			if (skinSystem->applySkin(skins[1].c_str()))
			{
				// Convert and apply special color
				stringw spccolor = skinSystem->getProperty(L"SpecialColor");
				if (!spccolor.empty())
				{
					unsigned int x;
					std::wstringstream ss;
					ss << std::hex << spccolor.c_str();
					ss >> x;
					if (!ss.fail())
						special_color = x;
				}
			}
		}
	}

	char* languague = new char[10];
	wcstombs(languague, gameConf.language, 10);
	char* sfile = new char[20];
	char* cfile = new char[20];
	strcpy(sfile, "data/"); // copy string one into the result.
	strcat(sfile, languague);
	strcat(sfile, "/cards.db");
	strcpy(cfile, "data/"); // copy string one into the result.
	strcat(cfile, languague);
	strcat(cfile, "/strings.conf");
	linePattern = 0x0f0f;
	waitFrame = 0;
	signalFrame = 0;
	showcard = 0;
	is_attacking = false;
	lpframe = 0;
	lpcstring = 0;
	always_chain = false;
	ignore_chain = false;
	is_building = false;
	memset(&dInfo, 0, sizeof(DuelInfo));
	memset(chatTiming, 0, sizeof(chatTiming));
	deckManager.LoadLFList();
	driver = device->getVideoDriver();
	driver->setTextureCreationFlag(irr::video::ETCF_CREATE_MIP_MAPS, false);
	driver->setTextureCreationFlag(irr::video::ETCF_OPTIMIZED_FOR_QUALITY, true);
	imageManager.SetDevice(device);
	if(!imageManager.Initial())
		return false;
	if(!dataManager.LoadDB(sfile))
		return false;
	if(!dataManager.LoadStrings(cfile))
		return false;
	env = device->getGUIEnvironment();
	numFont = irr::gui::CGUITTFont::createTTFont(env, gameConf.numfont, 16);
	adFont = irr::gui::CGUITTFont::createTTFont(env, gameConf.numfont, 12);
	lpcFont = irr::gui::CGUITTFont::createTTFont(env, gameConf.numfont, 48);
	guiFont = irr::gui::CGUITTFont::createTTFont(env, gameConf.textfont, gameConf.textfontsize);
	textFont = guiFont;
	smgr = device->getSceneManager();
	device->setWindowCaption(L"YGOPC");
	device->setResizable(true);

	//main menu
	wchar_t strbuf[256];
	myswprintf(strbuf, L"Update 1(Version:%X.0%X)", PRO_VERSION >> 12, PRO_VERSION & 0xf);
	wMainMenu = env->addWindow(rect<s32>(370, 200, 650, 450), false, strbuf);
	wMainMenu->getCloseButton()->setVisible(false);
	lastMenu = wMainMenu;
	btnOnlineMode = env->addButton(rect<s32>(10, 30, 270, 60), wMainMenu, BUTTON_ONLINE_MODE, dataManager.GetSysString(2044));
	btnLanMode = env->addButton(rect<s32>(10, 65, 270, 95), wMainMenu, BUTTON_LAN_MODE, dataManager.GetSysString(1200));
	btnServerMode = env->addButton(rect<s32>(10, 170, 270, 200), wMainMenu, BUTTON_SINGLE_MODE, dataManager.GetSysString(1201));
	btnReplayMode = env->addButton(rect<s32>(10, 135, 270, 165), wMainMenu, BUTTON_REPLAY_MODE, dataManager.GetSysString(1202));
	btnDeckEdit = env->addButton(rect<s32>(10, 100, 270, 130), wMainMenu, BUTTON_DECK_EDIT, dataManager.GetSysString(1204));
	btnModeExit = env->addButton(rect<s32>(10, 205, 270, 235), wMainMenu, BUTTON_MODE_EXIT, dataManager.GetSysString(1210));
	//campaign mode
	wCampaignMenu = env->addWindow(rect<s32>(220, 100, 800, 520), false, dataManager.GetSysString(2044));
	wCampaignMenu->getCloseButton()->setVisible(false);
	wCampaignMenu->setVisible(false);
	lstCPlayList = env->addListBox(rect<s32>(10, 120, 320, 400), wCampaignMenu, LISTBOX_SINGLEPLAY_LIST, true);
	lstCPlayList->setItemHeight(18);
	env->addStaticText(dataManager.GetSysString(1354), rect<s32>(10, 30, 90, 80), false, true, wCampaignMenu);
	cRaceC = env->addComboBox(rect<s32>(120, 30, 240, 60), wCampaignMenu, COMBOBOX_CTYPE);
	cRaceC->addItem(dataManager.GetSysString(1380), 0);
	cRaceC->addItem(dataManager.GetSysString(1381), 1);
	cRaceC->addItem(dataManager.GetSysString(1382), 2);
	cRaceC->addItem(dataManager.GetSysString(1383), 3);
	btnStartC = env->addButton(rect<s32>(460, 355, 570, 380), wCampaignMenu, BUTTON_CAMPAIGN_JOIN, dataManager.GetSysString(1211));
	btnCancelC = env->addButton(rect<s32>(460, 385, 570, 410), wCampaignMenu, BUTTON_CAMPAIGN_CANCEL, dataManager.GetSysString(1210));
	env->addStaticText(dataManager.GetSysString(1352), rect<s32>(360, 30, 570, 50), false, true, wCampaignMenu);
	stProgress = env->addStaticText(dataManager.GetSysString(1368), rect<s32>(360, 130, 570, 150), false, true, wCampaignMenu);
	stProgressC = env->addStaticText(L"0", rect<s32>(360, 170, 400, 200), false, true, wCampaignMenu);
	stProgressT = env->addStaticText(L" / 20", rect<s32>(410, 170, 570, 200), false, true, wCampaignMenu);
	stStatus = env->addStaticText(dataManager.GetSysString(1385), rect<s32>(360, 230, 570, 250), false, true, wCampaignMenu);
	stStatusC = env->addStaticText(dataManager.GetSysString(1387), rect<s32>(360, 270, 570, 300), false, true, wCampaignMenu);
	stProgress->setVisible(true);
	stProgressC->setVisible(true);
	stProgressT->setVisible(true);
	stStatus->setVisible(false);
	stStatusC->setVisible(false);
	stSinglePlayInfo = env->addStaticText(L"", rect<s32>(360, 60, 570, 350), false, true, wCampaignMenu);
	//lan mode
	wLanWindow = env->addWindow(rect<s32>(220, 100, 800, 520), false, dataManager.GetSysString(1200));
	wLanWindow->getCloseButton()->setVisible(false);
	wLanWindow->setVisible(false);
	env->addStaticText(dataManager.GetSysString(1220), rect<s32>(10, 30, 220, 50), false, false, wLanWindow);
	ebNickName = env->addEditBox(gameConf.nickname, rect<s32>(110, 25, 450, 50), true, wLanWindow, EDITBOX_NICKNAME_LAN);
	ebNickName->setTextAlignment(irr::gui::EGUIA_UPPERLEFT, irr::gui::EGUIA_CENTER);
	lstHostList = env->addListBox(rect<s32>(10, 60, 570, 320), wLanWindow, LISTBOX_LAN_HOST, true);
	lstHostList->setItemHeight(18);
	btnLanRefresh = env->addButton(rect<s32>(240, 325, 340, 350), wLanWindow, BUTTON_LAN_REFRESH, dataManager.GetSysString(1217));
	env->addStaticText(dataManager.GetSysString(1221), rect<s32>(10, 360, 220, 380), false, false, wLanWindow);
	ebJoinIP = env->addEditBox(gameConf.lastip, rect<s32>(110, 355, 250, 380), true, wLanWindow);
	ebJoinIP->setTextAlignment(irr::gui::EGUIA_CENTER, irr::gui::EGUIA_CENTER);
	ebJoinPort = env->addEditBox(gameConf.lastport, rect<s32>(260, 355, 320, 380), true, wLanWindow);
	ebJoinPort->setTextAlignment(irr::gui::EGUIA_CENTER, irr::gui::EGUIA_CENTER);
	env->addStaticText(dataManager.GetSysString(1222), rect<s32>(10, 390, 220, 410), false, false, wLanWindow);
	ebJoinPass = env->addEditBox(gameConf.roompass, rect<s32>(110, 385, 250, 410), true, wLanWindow);
	ebJoinPass->setTextAlignment(irr::gui::EGUIA_CENTER, irr::gui::EGUIA_CENTER);
	btnJoinHost = env->addButton(rect<s32>(460, 355, 570, 380), wLanWindow, BUTTON_JOIN_HOST, dataManager.GetSysString(1223));
	btnJoinCancel = env->addButton(rect<s32>(460, 385, 570, 410), wLanWindow, BUTTON_JOIN_CANCEL, dataManager.GetSysString(1212));
	btnCreateHost = env->addButton(rect<s32>(460, 25, 570, 50), wLanWindow, BUTTON_CREATE_HOST, dataManager.GetSysString(1224));
	//create host
	wCreateHost = env->addWindow(rect<s32>(320, 100, 700, 520), false, dataManager.GetSysString(1224));
	wCreateHost->getCloseButton()->setVisible(false);
	wCreateHost->setVisible(false);
	env->addStaticText(dataManager.GetSysString(1226), rect<s32>(20, 30, 220, 50), false, false, wCreateHost);
	cbLFlist = env->addComboBox(rect<s32>(140, 25, 300, 50), wCreateHost);
	for(unsigned int i = 0; i < deckManager._lfList.size(); ++i)
		cbLFlist->addItem(deckManager._lfList[i].listName, deckManager._lfList[i].hash);
	env->addStaticText(dataManager.GetSysString(1225), rect<s32>(20, 60, 220, 80), false, false, wCreateHost);
	cbRule = env->addComboBox(rect<s32>(140, 55, 300, 80), wCreateHost);
	cbRule->addItem(dataManager.GetSysString(1240));
	cbRule->addItem(dataManager.GetSysString(1241));
	cbRule->addItem(dataManager.GetSysString(1242));
	cbRule->addItem(dataManager.GetSysString(1243));
	env->addStaticText(dataManager.GetSysString(1227), rect<s32>(20, 90, 220, 110), false, false, wCreateHost);
	cbMatchMode = env->addComboBox(rect<s32>(140, 85, 300, 110), wCreateHost);
	cbMatchMode->addItem(dataManager.GetSysString(1244));
	cbMatchMode->addItem(dataManager.GetSysString(1245));
	cbMatchMode->addItem(dataManager.GetSysString(1246));
	env->addStaticText(dataManager.GetSysString(1237), rect<s32>(20, 120, 320, 140), false, false, wCreateHost);
	myswprintf(strbuf, L"%d", 180);
	ebTimeLimit = env->addEditBox(strbuf, rect<s32>(140, 115, 220, 140), true, wCreateHost);
	ebTimeLimit->setTextAlignment(irr::gui::EGUIA_CENTER, irr::gui::EGUIA_CENTER);
	env->addStaticText(dataManager.GetSysString(1228), rect<s32>(20, 150, 320, 170), false, false, wCreateHost);
	chkEnablePriority = env->addCheckBox(false, rect<s32>(20, 180, 360, 200), wCreateHost, -1, dataManager.GetSysString(1236));
	chkNoCheckDeck = env->addCheckBox(false, rect<s32>(20, 210, 170, 230), wCreateHost, -1, dataManager.GetSysString(1229));
	chkNoShuffleDeck = env->addCheckBox(false, rect<s32>(180, 210, 360, 230), wCreateHost, -1, dataManager.GetSysString(1230));
	env->addStaticText(dataManager.GetSysString(1231), rect<s32>(20, 240, 320, 260), false, false, wCreateHost);
	myswprintf(strbuf, L"%d", 40);
	ebStartLP = env->addEditBox(strbuf, rect<s32>(140, 235, 220, 260), true, wCreateHost);
	ebStartLP->setTextAlignment(irr::gui::EGUIA_CENTER, irr::gui::EGUIA_CENTER);
	env->addStaticText(dataManager.GetSysString(1232), rect<s32>(20, 270, 320, 290), false, false, wCreateHost);
	myswprintf(strbuf, L"%d", 5);
	ebStartHand = env->addEditBox(strbuf, rect<s32>(140, 265, 220, 290), true, wCreateHost);
	ebStartHand->setTextAlignment(irr::gui::EGUIA_CENTER, irr::gui::EGUIA_CENTER);
	env->addStaticText(dataManager.GetSysString(1233), rect<s32>(20, 300, 320, 320), false, false, wCreateHost);
	myswprintf(strbuf, L"%d", 1);
	ebDrawCount = env->addEditBox(strbuf, rect<s32>(140, 295, 220, 320), true, wCreateHost);
	ebDrawCount->setTextAlignment(irr::gui::EGUIA_CENTER, irr::gui::EGUIA_CENTER);
	env->addStaticText(dataManager.GetSysString(1234), rect<s32>(10, 360, 220, 380), false, false, wCreateHost);
	ebServerName = env->addEditBox(gameConf.gamename, rect<s32>(110, 355, 250, 380), true, wCreateHost);
	ebServerName->setTextAlignment(irr::gui::EGUIA_CENTER, irr::gui::EGUIA_CENTER);
	env->addStaticText(dataManager.GetSysString(1235), rect<s32>(10, 390, 220, 410), false, false, wCreateHost);
	ebServerPass = env->addEditBox(L"", rect<s32>(110, 385, 250, 410), true, wCreateHost);
	ebServerPass->setTextAlignment(irr::gui::EGUIA_CENTER, irr::gui::EGUIA_CENTER);
	btnHostConfirm = env->addButton(rect<s32>(260, 355, 370, 380), wCreateHost, BUTTON_HOST_CONFIRM, dataManager.GetSysString(1211));
	btnHostCancel = env->addButton(rect<s32>(260, 385, 370, 410), wCreateHost, BUTTON_HOST_CANCEL, dataManager.GetSysString(1212));
	//host(single)
	wHostPrepare = env->addWindow(rect<s32>(270, 120, 750, 440), false, dataManager.GetSysString(1250));
	wHostPrepare->getCloseButton()->setVisible(false);
	wHostPrepare->setVisible(false);
	btnHostPrepDuelist = env->addButton(rect<s32>(10, 30, 110, 55), wHostPrepare, BUTTON_HP_DUELIST, dataManager.GetSysString(1251));
	for(int i = 0; i < 2; ++i) {
		stHostPrepDuelist[i] = env->addStaticText(L"", rect<s32>(40, 65 + i * 25, 240, 85 + i * 25), true, false, wHostPrepare);
		btnHostPrepKick[i] = env->addButton(rect<s32>(10, 65 + i * 25, 30, 85 + i * 25), wHostPrepare, BUTTON_HP_KICK, L"X");
		chkHostPrepReady[i] = env->addCheckBox(false, rect<s32>(250, 65 + i * 25, 270, 85 + i * 25), wHostPrepare, CHECKBOX_HP_READY, L"");
		chkHostPrepReady[i]->setEnabled(false);
	}
	for(int i = 2; i < 4; ++i) {
		stHostPrepDuelist[i] = env->addStaticText(L"", rect<s32>(40, 75 + i * 25, 240, 95 + i * 25), true, false, wHostPrepare);
		btnHostPrepKick[i] = env->addButton(rect<s32>(10, 75 + i * 25, 30, 95 + i * 25), wHostPrepare, BUTTON_HP_KICK, L"X");
		chkHostPrepReady[i] = env->addCheckBox(false, rect<s32>(250, 75 + i * 25, 270, 95 + i * 25), wHostPrepare, CHECKBOX_HP_READY, L"");
		chkHostPrepReady[i]->setEnabled(false);
	}
	btnHostPrepOB = env->addButton(rect<s32>(10, 180, 110, 205), wHostPrepare, BUTTON_HP_OBSERVER, dataManager.GetSysString(1252));
	myswprintf(dataManager.strBuffer, L"%ls%d", dataManager.GetSysString(1253), 0);
	stHostPrepOB = env->addStaticText(dataManager.strBuffer, rect<s32>(10, 210, 270, 230), false, false, wHostPrepare);
	stHostPrepRule = env->addStaticText(L"", rect<s32>(280, 30, 460, 230), false, true, wHostPrepare);
	env->addStaticText(dataManager.GetSysString(1254), rect<s32>(10, 235, 110, 255), false, false, wHostPrepare);
	cbDeckSelect = env->addComboBox(rect<s32>(120, 230, 270, 255), wHostPrepare);
	btnHostPrepStart = env->addButton(rect<s32>(230, 280, 340, 305), wHostPrepare, BUTTON_HP_START, dataManager.GetSysString(1215));
	btnHostPrepCancel = env->addButton(rect<s32>(350, 280, 460, 305), wHostPrepare, BUTTON_HP_CANCEL, dataManager.GetSysString(1212));
	//img
	wCardImg = env->addStaticText(L"", rect<s32>(1, 1, 199, 273), true, false, 0, -1, true);
	wCardImg->setBackgroundColor(0xc0c0c0c0);
	wCardImg->setVisible(false);
	imgCard = env->addImage(rect<s32>(9, 9, 187, 262), wCardImg);
	imgCard->setUseAlphaChannel(true);
	//phase
	wPhase = env->addStaticText(L"", rect<s32>(480, 310, 855, 330));
	wPhase->setVisible(false);
	btnDP = env->addButton(rect<s32>(0, 0, 50, 20), wPhase, -1, L"\xff24\xff30");
	btnDP->setEnabled(false);
	btnDP->setPressed(true);
	btnDP->setVisible(false);
	btnSP = env->addButton(rect<s32>(65, 0, 115, 20), wPhase, -1, L"\xff33\xff30");
	btnSP->setEnabled(false);
	btnSP->setPressed(true);
	btnSP->setVisible(false);
	btnM1 = env->addButton(rect<s32>(130, 0, 180, 20), wPhase, -1, L"\xff2d\xff11");
	btnM1->setEnabled(false);
	btnM1->setPressed(true);
	btnM1->setVisible(false);
	btnBP = env->addButton(rect<s32>(195, 0, 245, 20), wPhase, BUTTON_BP, L"\xff22\xff30");
	btnBP->setVisible(false);
	btnM2 = env->addButton(rect<s32>(260, 0, 310, 20), wPhase, BUTTON_M2, L"\xff2d\xff12");
	btnM2->setVisible(false);
	btnEP = env->addButton(rect<s32>(325, 0, 375, 20), wPhase, BUTTON_EP, L"\xff25\xff30");
	btnEP->setVisible(false);
	//avatar
	//imgAvatar = env->addImage(rect<s32>(200, 40, 82, 82), false);
	//imgAvatar-> setImage(driver->getTexture("pics/avatar/avatar.png"));

	//tab
	wInfos = env->addTabControl(rect<s32>(1, 275, 301, 639), 0, true);
	wInfos->setVisible(false);
	//info
	irr::gui::IGUITab* tabInfo = wInfos->addTab(dataManager.GetSysString(1270));
	stName = env->addStaticText(L"", rect<s32>(10, 10, 287, 32), true, false, tabInfo, -1, false);
	stName->setTextAlignment(irr::gui::EGUIA_CENTER, irr::gui::EGUIA_CENTER);
	stInfo = env->addStaticText(L"", rect<s32>(15, 37, 296, 60), false, true, tabInfo, -1, false);
	stInfo->setOverrideColor(special_color);
	stDataInfo = env->addStaticText(L"", rect<s32>(15, 60, 296, 83), false, true, tabInfo, -1, false);
	stDataInfo->setOverrideColor(special_color);
	stText = env->addStaticText(L"", rect<s32>(15, 83, 287, 324), false, true, tabInfo, -1, false);
	scrCardText = env->addScrollBar(false, rect<s32>(267, 83, 287, 324), tabInfo, SCROLL_CARDTEXT);
	scrCardText->setLargeStep(1);
	scrCardText->setSmallStep(1);
	scrCardText->setVisible(false);
	//log
	irr::gui::IGUITab* tabLog =  wInfos->addTab(dataManager.GetSysString(1271));
	lstLog = env->addListBox(rect<s32>(10, 10, 290, 290), tabLog, LISTBOX_LOG, false);
	lstLog->setItemHeight(18);
	btnClearLog = env->addButton(rect<s32>(160, 300, 260, 325), tabLog, BUTTON_CLEAR_LOG, dataManager.GetSysString(1272));
	//system
	irr::gui::IGUITab* tabSystem = wInfos->addTab(dataManager.GetSysString(1273));
	chkAutoPos = env->addCheckBox(gameConf.autoplace, rect<s32>(20, 20, 280, 45), tabSystem, -1, dataManager.GetSysString(1274));
	chkRandomPos = env->addCheckBox(gameConf.randomplace, rect<s32>(40, 50, 300, 75), tabSystem, -1, dataManager.GetSysString(1275));
	chkAutoChain = env->addCheckBox(gameConf.autochain, rect<s32>(20, 80, 280, 105), tabSystem, -1, dataManager.GetSysString(1276));
	chkWaitChain = env->addCheckBox(false, rect<s32>(20, 110, 280, 135), tabSystem, -1, dataManager.GetSysString(1277));
	chkIgnore1 = env->addCheckBox(false, rect<s32>(20, 170, 280, 195), tabSystem, -1, dataManager.GetSysString(1290));
	chkIgnore2 = env->addCheckBox(false, rect<s32>(20, 200, 280, 225), tabSystem, -1, dataManager.GetSysString(1291));
	chkEnableSound = env->addCheckBox(gameConf.enablesound, rect<s32>(20, 230, 280, 255), tabSystem, CHECKBOX_ENABLE_SOUND, dataManager.GetSysString(2046));
	scrSound = env->addScrollBar(true, rect<s32>(20, 260, 280, 270), tabSystem, SCROLL_SOUND);
	scrSound->setMax(100);
	scrSound->setMin(0);
	scrSound->setPos(gameConf.soundvolume);
	scrSound->setLargeStep(1);
	scrSound->setSmallStep(1);
	chkEnableMusic  = env->addCheckBox(gameConf.enablemusic, rect<s32>(20, 275, 280, 300), tabSystem, CHECKBOX_ENABLE_MUSIC, dataManager.GetSysString(2047));
	scrMusic = env->addScrollBar(true, rect<s32>(20, 305, 280, 315), tabSystem, SCROLL_MUSIC);
	scrMusic->setMax(100);
	scrMusic->setMin(0);
	scrMusic->setPos(gameConf.musicvolume);
	scrMusic->setLargeStep(1);
	scrMusic->setSmallStep(1);
	//
	wHand = env->addWindow(rect<s32>(500, 450, 825, 605), false, L"");
	wHand->getCloseButton()->setVisible(false);
	wHand->setDraggable(false);
	wHand->setDrawTitlebar(false);
	wHand->setVisible(false);
	for(int i = 0; i < 3; ++i) {
		btnHand[i] = env->addButton(rect<s32>(10 + 105 * i, 10, 105 + 105 * i, 144), wHand, BUTTON_HAND1 + i, L"");
		btnHand[i]->setImage(imageManager.tHand[i]);
	}
	//
	wFTSelect = env->addWindow(rect<s32>(550, 240, 780, 340), false, L"");
	wFTSelect->getCloseButton()->setVisible(false);
	wFTSelect->setVisible(false);
	btnFirst = env->addButton(rect<s32>(10, 30, 220, 55), wFTSelect, BUTTON_FIRST, dataManager.GetSysString(94));
	btnSecond = env->addButton(rect<s32>(10, 60, 220, 85), wFTSelect, BUTTON_SECOND, dataManager.GetSysString(95));
	//message (310)
	wMessage = env->addWindow(rect<s32>(490, 200, 840, 340), false, dataManager.GetSysString(1216));
	wMessage->getCloseButton()->setVisible(false);
	wMessage->setVisible(false);
	stMessage =  env->addStaticText(L"", rect<s32>(20, 20, 350, 100), false, true, wMessage, -1, false);
	stMessage->setTextAlignment(irr::gui::EGUIA_UPPERLEFT, irr::gui::EGUIA_CENTER);
	btnMsgOK = env->addButton(rect<s32>(130, 105, 220, 130), wMessage, BUTTON_MSG_OK, dataManager.GetSysString(1211));
	//auto fade message (310)
	wACMessage = env->addWindow(rect<s32>(490, 240, 840, 300), false, L"");
	wACMessage->getCloseButton()->setVisible(false);
	wACMessage->setVisible(false);
	wACMessage->setDrawBackground(false);
	stACMessage = env->addStaticText(L"", rect<s32>(0, 0, 350, 60), true, true, wACMessage, -1, true);
	stACMessage->setBackgroundColor(0xc0c0c0ff);
	stACMessage->setTextAlignment(irr::gui::EGUIA_CENTER, irr::gui::EGUIA_CENTER);
	//yes/no (310)
	wQuery = env->addWindow(rect<s32>(490, 200, 840, 340), false, dataManager.GetSysString(560));
	wQuery->getCloseButton()->setVisible(false);
	wQuery->setVisible(false);
	stQMessage =  env->addStaticText(L"", rect<s32>(20, 20, 350, 100), false, true, wQuery, -1, false);
	stQMessage->setTextAlignment(irr::gui::EGUIA_UPPERLEFT, irr::gui::EGUIA_CENTER);
	btnYes = env->addButton(rect<s32>(100, 105, 150, 130), wQuery, BUTTON_YES, dataManager.GetSysString(1213));
	btnNo = env->addButton(rect<s32>(200, 105, 250, 130), wQuery, BUTTON_NO, dataManager.GetSysString(1214));
	//options (310)
	wOptions = env->addWindow(rect<s32>(490, 200, 840, 340), false, L"");
	wOptions->getCloseButton()->setVisible(false);
	wOptions->setVisible(false);
	stOptions =  env->addStaticText(L"", rect<s32>(20, 20, 350, 100), false, true, wOptions, -1, false);
	stOptions->setTextAlignment(irr::gui::EGUIA_UPPERLEFT, irr::gui::EGUIA_CENTER);
	btnOptionOK = env->addButton(rect<s32>(130, 105, 220, 130), wOptions, BUTTON_OPTION_OK, dataManager.GetSysString(1211));
	btnOptionp = env->addButton(rect<s32>(20, 105, 60, 130), wOptions, BUTTON_OPTION_PREV, L"<<<");
	btnOptionn = env->addButton(rect<s32>(290, 105, 330, 130), wOptions, BUTTON_OPTION_NEXT, L">>>");
	//pos select
	wPosSelect = env->addWindow(rect<s32>(340, 200, 935, 410), false, dataManager.GetSysString(561));
	wPosSelect->getCloseButton()->setVisible(false);
	wPosSelect->setVisible(false);
	btnPSAU = irr::gui::CGUIImageButton::addImageButton(env, rect<s32>(10, 45, 150, 185), wPosSelect, BUTTON_POS_AU);
	btnPSAU->setImageScale(core::vector2df(0.5, 0.5));
	btnPSAD = irr::gui::CGUIImageButton::addImageButton(env, rect<s32>(155, 45, 295, 185), wPosSelect, BUTTON_POS_AD);
	btnPSAD->setImageScale(core::vector2df(0.5, 0.5));
	btnPSAD->setImage(imageManager.tCover[0], rect<s32>(0, 0, 177, 254));
	btnPSDU = irr::gui::CGUIImageButton::addImageButton(env, rect<s32>(300, 45, 440, 185), wPosSelect, BUTTON_POS_DU);
	btnPSDU->setImageScale(core::vector2df(0.5, 0.5));
	btnPSDU->setImageRotation(270);
	btnPSDD = irr::gui::CGUIImageButton::addImageButton(env, rect<s32>(445, 45, 585, 185), wPosSelect, BUTTON_POS_DD);
	btnPSDD->setImageScale(core::vector2df(0.5, 0.5));
	btnPSDD->setImageRotation(270);
	btnPSDD->setImage(imageManager.tCover[0], rect<s32>(0, 0, 177, 254));
	//card select
	wCardSelect = env->addWindow(rect<s32>(320, 100, 1000, 400), false, L"");
	wCardSelect->getCloseButton()->setVisible(false);
	wCardSelect->setVisible(false);
	for(int i = 0; i < 5; ++i) {
		stCardPos[i] = env->addStaticText(L"", rect<s32>(40 + 125 * i, 30, 139 + 125 * i, 50), true, false, wCardSelect, -1, true);
		stCardPos[i]->setBackgroundColor(0xffffffff);
		stCardPos[i]->setTextAlignment(irr::gui::EGUIA_CENTER, irr::gui::EGUIA_CENTER);
		btnCardSelect[i] = irr::gui::CGUIImageButton::addImageButton(env, rect<s32>(30 + 125 * i, 55, 150 + 125 * i, 225), wCardSelect, BUTTON_CARD_0 + i);
		btnCardSelect[i]->setImageScale(core::vector2df(0.6f, 0.6f));
	}
	scrCardList = env->addScrollBar(true, rect<s32>(30, 235, 650, 255), wCardSelect, SCROLL_CARD_SELECT);
	btnSelectOK = env->addButton(rect<s32>(300, 265, 380, 290), wCardSelect, BUTTON_CARD_SEL_OK, dataManager.GetSysString(1211));
	//announce number
	wANNumber = env->addWindow(rect<s32>(550, 200, 780, 295), false, L"");
	wANNumber->getCloseButton()->setVisible(false);
	wANNumber->setVisible(false);
	cbANNumber =  env->addComboBox(rect<s32>(40, 30, 190, 50), wANNumber, -1);
	cbANNumber->setTextAlignment(irr::gui::EGUIA_CENTER, irr::gui::EGUIA_CENTER);
	btnANNumberOK = env->addButton(rect<s32>(80, 60, 150, 85), wANNumber, BUTTON_ANNUMBER_OK, dataManager.GetSysString(1211));
	//announce card
	wANCard = env->addWindow(rect<s32>(560, 170, 770, 370), false, L"");
	wANCard->getCloseButton()->setVisible(false);
	wANCard->setVisible(false);
	ebANCard = env->addEditBox(L"", rect<s32>(20, 25, 190, 45), true, wANCard, EDITBOX_ANCARD);
	ebANCard->setTextAlignment(irr::gui::EGUIA_CENTER, irr::gui::EGUIA_CENTER);
	lstANCard = env->addListBox(rect<s32>(20, 50, 190, 160), wANCard, LISTBOX_ANCARD, true);
	btnANCardOK = env->addButton(rect<s32>(60, 165, 150, 190), wANCard, BUTTON_ANCARD_OK, dataManager.GetSysString(1211));
	//announce attribute
	wANAttribute = env->addWindow(rect<s32>(500, 200, 830, 285), false, dataManager.GetSysString(562));
	wANAttribute->getCloseButton()->setVisible(false);
	wANAttribute->setVisible(false);
	for(int filter = 0x1, i = 0; i < 7; filter <<= 1, ++i)
		chkAttribute[i] = env->addCheckBox(false, rect<s32>(10 + (i % 4) * 80, 25 + (i / 4) * 25, 90 + (i % 4) * 80, 50 + (i / 4) * 25),
		                                   wANAttribute, CHECK_ATTRIBUTE, dataManager.FormatAttribute(filter));
	//announce attribute
	wANRace = env->addWindow(rect<s32>(480, 200, 850, 385), false, dataManager.GetSysString(563));
	wANRace->getCloseButton()->setVisible(false);
	wANRace->setVisible(false);
	for(int filter = 0x1, i = 0; i < 24; filter <<= 1, ++i)
		chkRace[i] = env->addCheckBox(false, rect<s32>(10 + (i % 4) * 90, 25 + (i / 4) * 25, 100 + (i % 4) * 90, 50 + (i / 4) * 25),
		                              wANRace, CHECK_RACE, dataManager.FormatRace(filter));
	//selection hint
	stHintMsg = env->addStaticText(L"", rect<s32>(500, 60, 820, 90), true, false, 0, -1, false);
	stHintMsg->setBackgroundColor(0xc0ffffff);
	stHintMsg->setTextAlignment(irr::gui::EGUIA_CENTER, irr::gui::EGUIA_CENTER);
	stHintMsg->setVisible(false);
	stTip = env->addStaticText(L"", rect<s32>(0, 0, 150, 150), false, true, 0, -1, true);
	stTip->setOverrideColor(0xff000000);
	stTip->setBackgroundColor(0xc0ffffff);
	stTip->setTextAlignment(irr::gui::EGUIA_CENTER, irr::gui::EGUIA_CENTER);
	stTip->setVisible(false);
	//cmd menu
	wCmdMenu = env->addWindow(rect<s32>(10, 10, 110, 179), false, L"");
	wCmdMenu->setDrawTitlebar(false);
	wCmdMenu->setVisible(false);
	wCmdMenu->getCloseButton()->setVisible(false);
	btnActivate = env->addButton(rect<s32>(1, 1, 99, 21), wCmdMenu, BUTTON_CMD_ACTIVATE, dataManager.GetSysString(1150));
	btnSummon = env->addButton(rect<s32>(1, 22, 99, 42), wCmdMenu, BUTTON_CMD_SUMMON, dataManager.GetSysString(1151));
	btnSPSummon = env->addButton(rect<s32>(1, 43, 99, 63), wCmdMenu, BUTTON_CMD_SPSUMMON, dataManager.GetSysString(1152));
	btnMSet = env->addButton(rect<s32>(1, 64, 99, 84), wCmdMenu, BUTTON_CMD_MSET, dataManager.GetSysString(1153));
	btnSSet = env->addButton(rect<s32>(1, 106, 99, 126), wCmdMenu, BUTTON_CMD_SSET, dataManager.GetSysString(1159));
	btnRepos = env->addButton(rect<s32>(1, 106, 99, 126), wCmdMenu, BUTTON_CMD_REPOS, dataManager.GetSysString(1154));
	btnAttack = env->addButton(rect<s32>(1, 127, 99, 147), wCmdMenu, BUTTON_CMD_ATTACK, dataManager.GetSysString(1157));
	btnShowList = env->addButton(rect<s32>(1, 148, 99, 168), wCmdMenu, BUTTON_CMD_SHOWLIST, dataManager.GetSysString(1158));
	//deck edit
	wDeckEdit = env->addStaticText(L"", rect<s32>(309, 8, 605, 130), true, false, 0, -1, true);
	wDeckEdit->setVisible(false);
	stLabel1 = env->addStaticText(dataManager.GetSysString(1300), rect<s32>(10, 9, 100, 29), false, false, wDeckEdit);
	cbDBLFList = env->addComboBox(rect<s32>(80, 5, 220, 30), wDeckEdit, COMBOBOX_DBLFLIST);
	stLabel2 = env->addStaticText(dataManager.GetSysString(1301), rect<s32>(10, 39, 100, 59), false, false, wDeckEdit);
	cbDBDecks = env->addComboBox(rect<s32>(80, 35, 220, 60), wDeckEdit, COMBOBOX_DBDECKS);
	for(unsigned int i = 0; i < deckManager._lfList.size(); ++i)
		cbDBLFList->addItem(deckManager._lfList[i].listName);
	btnSaveDeck = env->addButton(rect<s32>(225, 35, 290, 60), wDeckEdit, BUTTON_SAVE_DECK, dataManager.GetSysString(1302));
	ebDeckname = env->addEditBox(L"", rect<s32>(80, 65, 220, 90), true, wDeckEdit, -1);
	ebDeckname->setTextAlignment(irr::gui::EGUIA_CENTER, irr::gui::EGUIA_CENTER);
	btnSaveDeckAs = env->addButton(rect<s32>(225, 65, 290, 90), wDeckEdit, BUTTON_SAVE_DECK_AS, dataManager.GetSysString(1303));
	btnClearDeck = env->addButton(rect<s32>(240, 95, 290, 116), wDeckEdit, BUTTON_CLEAR_DECK, dataManager.GetSysString(1304));
	btnSortDeck = env->addButton(rect<s32>(185, 95, 235, 116), wDeckEdit, BUTTON_SORT_DECK, dataManager.GetSysString(1305));
	btnShuffleDeck = env->addButton(rect<s32>(130, 95, 180, 116), wDeckEdit, BUTTON_SHUFFLE_DECK, dataManager.GetSysString(1307));
	btnDBExit = env->addButton(rect<s32>(10, 95, 90, 116), wDeckEdit, BUTTON_DBEXIT, dataManager.GetSysString(1306));
	btnSideOK = env->addButton(rect<s32>(510, 40, 820, 80), 0, BUTTON_SIDE_OK, dataManager.GetSysString(1334));
	btnDeleteDeck = env->addButton(rect<s32>(10, 68, 75,89), wDeckEdit, BUTTON_DELETE_DECK, dataManager.GetSysString(2027));
	btnSideOK->setVisible(false);
	//filters
	wFilter = env->addStaticText(L"", rect<s32>(610, 8, 1020, 130), true, false, 0, -1, true);
	wFilter->setVisible(false);
	stLabel3 = env->addStaticText(dataManager.GetSysString(1311), rect<s32>(10, 5, 70, 25), false, false, wFilter);
	cbCardType = env->addComboBox(rect<s32>(80, 3, 150, 23), wFilter, COMBOBOX_MAINTYPE);
	cbCardType->addItem(dataManager.GetSysString(1310));
	cbCardType->addItem(dataManager.GetSysString(1312));
	cbCardType->addItem(dataManager.GetSysString(1313));
	cbCardType->addItem(dataManager.GetSysString(1314));
	cbCardType2 = env->addComboBox(rect<s32>(150, 3, 230, 23), wFilter, -1);
	cbCardType2->addItem(dataManager.GetSysString(1310), 0);
	stLabel4 = env->addStaticText(dataManager.GetSysString(1315), rect<s32>(240, 5, 290, 25), false, false, wFilter);
	cbLimit = env->addComboBox(rect<s32>(300, 3, 390, 23), wFilter, -1);
	cbLimit->addItem(dataManager.GetSysString(1310));
	cbLimit->addItem(dataManager.GetSysString(1316));
	cbLimit->addItem(dataManager.GetSysString(1317));
	//cbLimit->addItem(dataManager.GetSysString(1318));
	stLabel5 = env->addStaticText(dataManager.GetSysString(1319), rect<s32>(10, 28, 70, 48), false, false, wFilter);
	cbAttribute = env->addComboBox(rect<s32>(80, 26, 170, 46), wFilter, -1);
	cbAttribute->addItem(dataManager.GetSysString(1310), 0);
		for(int filter = 0x1; filter != 0x80; filter <<= 1)
			cbAttribute->addItem(dataManager.FormatAttribute(filter), filter);
	stLabel6 = env->addStaticText(dataManager.GetSysString(1321), rect<s32>(10, 51, 70, 71), false, false, wFilter);
	cbRace = env->addComboBox(rect<s32>(80, 49, 170, 69), wFilter, -1);
	cbRace->addItem(dataManager.GetSysString(1310), 0);
		for(int filter = 0x1; filter != 0x80000; filter <<= 1)
			cbRace->addItem(dataManager.FormatRace(filter), filter);
	stLabel7 = env->addStaticText(dataManager.GetSysString(1322), rect<s32>(240, 28, 290, 48), false, false, wFilter);
	ebAttack = env->addEditBox(L"", rect<s32>(320, 26, 380, 46), true, wFilter);
	ebAttack->setTextAlignment(irr::gui::EGUIA_CENTER, irr::gui::EGUIA_CENTER);
	stLabel8 = env->addStaticText(dataManager.GetSysString(1323), rect<s32>(240, 51, 290, 71), false, false, wFilter);
	ebDefence = env->addEditBox(L"", rect<s32>(320, 49, 380, 69), true, wFilter);
	ebDefence->setTextAlignment(irr::gui::EGUIA_CENTER, irr::gui::EGUIA_CENTER);
	stLabel9 = env->addStaticText(dataManager.GetSysString(1324), rect<s32>(10, 74, 80, 94), false, false, wFilter);
	ebStar = env->addEditBox(L"", rect<s32>(80, 72, 160, 92), true, wFilter);
	ebStar->setTextAlignment(irr::gui::EGUIA_CENTER, irr::gui::EGUIA_CENTER);
	stLabel10 = env->addStaticText(dataManager.GetSysString(1325), rect<s32>(205, 74, 280, 94), false, false, wFilter);
	ebCardName = env->addEditBox(L"", rect<s32>(260, 72, 390, 92), true, wFilter, EDITBOX_KEYWORD);
	ebCardName->setTextAlignment(irr::gui::EGUIA_CENTER, irr::gui::EGUIA_CENTER);
	//btnEffectFilter = env->addButton(rect<s32>(345, 28, 390, 69), wFilter, BUTTON_EFFECT_FILTER, dataManager.GetSysString(1326));
	btnStartFilter = env->addButton(rect<s32>(260, 96, 390, 118), wFilter, BUTTON_START_FILTER, dataManager.GetSysString(1327));
	btnClearFilter = env->addButton(rect<s32>(205, 96, 255, 118), wFilter, BUTTON_CLEAR_FILTER, dataManager.GetSysString(1304));
	wCategories = env->addWindow(rect<s32>(450, 60, 1000, 270), false, dataManager.strBuffer);
	wCategories->getCloseButton()->setVisible(false);
	wCategories->setDrawTitlebar(false);
	wCategories->setDraggable(false);
	wCategories->setVisible(false);
	btnCategoryOK = env->addButton(rect<s32>(200, 175, 300, 200), wCategories, BUTTON_CATEGORY_OK, dataManager.GetSysString(1211));
	for(int i = 0; i < 32; ++i)
		chkCategory[i] = env->addCheckBox(false, recti(10 + (i % 4) * 130, 10 + (i / 4) * 20, 140 + (i % 4) * 130, 30 + (i / 4) * 20), wCategories, -1, dataManager.GetSysString(1100 + i));
	scrFilter = env->addScrollBar(false, recti(999, 161, 1019, 629), 0, SCROLL_FILTER);
	scrFilter->setLargeStep(10);
	scrFilter->setSmallStep(1);
	scrFilter->setVisible(false);
	//cbSetCode = env->addComboBox(rect<s32>(60, 96, 190, 118), wFilter, -1);
	//cbSetCode->addItem(L"None", 0);
	//std::vector<wchar_t*> setcodes = dataManager.GetSetcodeList();
	//for (int i = 0; i < (int)setcodes.size(); ++i)
		//cbSetCode->addItem(setcodes[i], dataManager.GetSetcode(setcodes[i]));
	//stLabel11 = env->addStaticText(dataManager.GetSysString(1393), rect<s32>(10, 100, 70, 122), false, false, wFilter);

	//replay window
	wReplay = env->addWindow(rect<s32>(220, 100, 800, 520), false, dataManager.GetSysString(1202));
	wReplay->getCloseButton()->setVisible(false);
	wReplay->setVisible(false);
	lstReplayList = env->addListBox(rect<s32>(10, 30, 350, 400), wReplay, LISTBOX_REPLAY_LIST, true);
	lstReplayList->setItemHeight(18);
	btnLoadReplay = env->addButton(rect<s32>(460, 355, 570, 380), wReplay, BUTTON_LOAD_REPLAY, dataManager.GetSysString(1348));
	btnReplayCancel = env->addButton(rect<s32>(460, 385, 570, 410), wReplay, BUTTON_CANCEL_REPLAY, dataManager.GetSysString(1347));
	env->addStaticText(dataManager.GetSysString(1349), rect<s32>(360, 30, 570, 50), false, true, wReplay);
	stReplayInfo = env->addStaticText(L"", rect<s32>(360, 60, 570, 350), false, true, wReplay);
	env->addStaticText(dataManager.GetSysString(1353), rect<s32>(360, 275, 570, 295), false, true, wReplay);
	ebRepStartTurn = env->addEditBox(L"", rect<s32>(360, 300, 460, 320), true, wReplay, -1);
	ebRepStartTurn->setTextAlignment(irr::gui::EGUIA_CENTER, irr::gui::EGUIA_CENTER);
	//configuration window
	wSinglePlay = env->addWindow(rect<s32>(220, 100, 800, 520), false, dataManager.GetSysString(1201));
	wSinglePlay->getCloseButton()->setVisible(false);
	wSinglePlay->setVisible(false);
	btnLoadSinglePlay = env->addButton(rect<s32>(320, 385, 430, 410), wSinglePlay, BUTTON_LOAD_SINGLEPLAY, dataManager.GetSysString(1211));
	btnSinglePlayCancel = env->addButton(rect<s32>(460, 385, 570, 410), wSinglePlay, BUTTON_CANCEL_SINGLEPLAY, dataManager.GetSysString(1210));
	env->addStaticText(dataManager.GetSysString(1356), rect<s32>(360, 30, 570, 50), false, true, wSinglePlay);
	chkEnableMods = env->addCheckBox(false, rect<s32>(360, 60, 570, 120), wSinglePlay, CHECKBOX_ENABLE_MODS, dataManager.GetSysString(1358));
	lstSinglePlayList = env->addListBox(rect<s32>(360, 130, 570, 330), wSinglePlay, LISTBOX_MODS_LIST, true);
	lstSinglePlayList->setItemHeight(18);	
	env->addStaticText(dataManager.GetSysString(1357), rect<s32>(10, 30, 80, 60), false, true, wSinglePlay);
	cbLanguageSelect = env->addComboBox(rect<s32>(10, 80, 110, 110), wSinglePlay, COMBOBOX_LANGUAGE);
	cbLanguageSelect->addItem(dataManager.GetSysString(1360), 0);
	cbLanguageSelect->addItem(dataManager.GetSysString(1361), 1);
	cbLanguageSelect->addItem(dataManager.GetSysString(1362), 2);
	cbLanguageSelect->addItem(dataManager.GetSysString(1363), 3);
	cbLanguageSelect->addItem(dataManager.GetSysString(1364), 4);
	cbLanguageSelect->addItem(dataManager.GetSysString(1365), 5);
	cbLanguageSelect->setText(mainGame->gameConf.language);
	env->addStaticText(dataManager.GetSysString(1359), rect<s32>(10, 130, 80, 160), false, true, wSinglePlay);
	cbRenderSelect = env->addComboBox(rect<s32>(10, 180, 110, 210), wSinglePlay, COMBOBOX_RENDERER);
	cbRenderSelect->addItem(dataManager.GetSysString(1370), 0);
	cbRenderSelect->addItem(dataManager.GetSysString(1371), 1);
	cbRenderSelect->setText(mainGame->gameConf.renderer);
	env->addStaticText(dataManager.GetSysString(1372), rect<s32>(10, 230, 80, 260), false, true, wSinglePlay);
	cbSkinSelect = env->addComboBox(rect<s32>(10, 280, 110, 310), wSinglePlay, COMBOBOX_SKIN);
	cbSkinSelect->addItem(dataManager.GetSysString(1374), 0);
	cbSkinSelect->addItem(dataManager.GetSysString(1375), 1);
	chkEnableAdvanceGraphics = env->addCheckBox(false, rect<s32>(10, 320, 210, 420), wSinglePlay, CHECKBOX_ENABLE_GRAPHICS, dataManager.GetSysString(1373));
	//replay save
	wReplaySave = env->addWindow(rect<s32>(510, 200, 820, 320), false, dataManager.GetSysString(1340));
	wReplaySave->getCloseButton()->setVisible(false);
	wReplaySave->setVisible(false);
	env->addStaticText(dataManager.GetSysString(1342), rect<s32>(20, 25, 290, 45), false, false, wReplaySave);
	ebRSName =  env->addEditBox(L"", rect<s32>(20, 50, 290, 70), true, wReplaySave, -1);
	ebRSName->setTextAlignment(irr::gui::EGUIA_CENTER, irr::gui::EGUIA_CENTER);
	btnRSYes = env->addButton(rect<s32>(70, 80, 140, 105), wReplaySave, BUTTON_REPLAY_SAVE, dataManager.GetSysString(1341));
	btnRSNo = env->addButton(rect<s32>(170, 80, 240, 105), wReplaySave, BUTTON_REPLAY_CANCEL, dataManager.GetSysString(1212));
	//replay control
	wReplayControl = env->addStaticText(L"", rect<s32>(205, 143, 295, 273), true, false, 0, -1, true);
	wReplayControl->setVisible(false);
	btnReplayStart = env->addButton(rect<s32>(5, 5, 85, 25), wReplayControl, BUTTON_REPLAY_START, dataManager.GetSysString(1343));
	btnReplayPause = env->addButton(rect<s32>(5, 30, 85, 50), wReplayControl, BUTTON_REPLAY_PAUSE, dataManager.GetSysString(1344));
	btnReplayStep = env->addButton(rect<s32>(5, 55, 85, 75), wReplayControl, BUTTON_REPLAY_STEP, dataManager.GetSysString(1345));
	btnReplaySwap = env->addButton(rect<s32>(5, 80, 85, 100), wReplayControl, BUTTON_REPLAY_SWAP, dataManager.GetSysString(1346));
	btnReplayExit = env->addButton(rect<s32>(5, 105, 85, 125), wReplayControl, BUTTON_REPLAY_EXIT, dataManager.GetSysString(1347));
	//chat
	wChat = env->addWindow(rect<s32>(305, 615, 1020, 640), false, L"");
	wChat->getCloseButton()->setVisible(false);
	wChat->setDraggable(false);
	wChat->setDrawTitlebar(false);
	wChat->setVisible(false);
	ebChatInput = env->addEditBox(L"", rect<s32>(3, 2, 710, 22), true, wChat, EDITBOX_CHAT);
	//
	btnLeaveGame = env->addButton(rect<s32>(205, 5, 295, 80), 0, BUTTON_LEAVE_GAME, L"");
	btnLeaveGame->setVisible(false);
	device->setEventReceiver(&menuHandler);
	env->getSkin()->setFont(guiFont);
	env->setFocus(wMainMenu);
	for (u32 i = 0; i < EGDC_COUNT; ++i) {
		SColor col = env->getSkin()->getColor((EGUI_DEFAULT_COLOR)i);
		col.setAlpha(224);
		env->getSkin()->setColor((EGUI_DEFAULT_COLOR)i, col);
	}
    engineSound = irrklang::createIrrKlangDevice();
    engineMusic = irrklang::createIrrKlangDevice(); 
	hideChat=false;
	hideChatTimer=0;


	return true;
}
void Game::MainLoop() {
	wchar_t cap[256];
	camera = smgr->addCameraSceneNode(0);
	irr::core::matrix4 mProjection;
	BuildProjectionMatrix(mProjection, -0.90f, 0.45f, -0.42f, 0.42f, 1.0f, 100.0f);
	camera->setProjectionMatrix(mProjection);

	mProjection.buildCameraLookAtMatrixLH(vector3df(4.2f, 8.0f, 7.8f), vector3df(4.2f, 0, 0), vector3df(0, 0, 1));
	camera->setViewMatrixAffector(mProjection);
	smgr->setAmbientLight(SColorf(1.0f, 1.0f, 1.0f));
	float atkframe = 0.1f;
	irr::ITimer* timer = device->getTimer();
	timer->setTime(0);
	int fps = 0;
	int cur_time = 0;
	while(device->run()) {
		dimension2du size = driver->getScreenSize();
		if (window_size != size) {
			window_size = size;
			OnResize();
		}
		if(gameConf.use_d3d)
			linePattern = (linePattern + 1) % 30;
		else
			linePattern = (linePattern << 1) | (linePattern >> 15);
		atkframe += 0.1f;
		atkdy = (float)sin(atkframe);
		driver->beginScene(true, true, SColor(0, 0, 0, 0));
		if(imageManager.tBackGround)
			driver->draw2DImage(imageManager.tBackGround, Resize(0, 0, 1024, 640), recti(0, 0, imageManager.tBackGround->getOriginalSize().Width, imageManager.tBackGround->getOriginalSize().Height));
		gMutex.Lock();
		if(dInfo.isStarted) {
	      if(imageManager.tBackGround2)
			driver->draw2DImage(imageManager.tBackGround2, Resize(0, 0, 1024, 640), recti(0, 0, imageManager.tBackGround->getOriginalSize().Width, imageManager.tBackGround->getOriginalSize().Height));
		  if(mainGame->showcardcode == 1 || mainGame->showcardcode == 3)
			  Game::PlayMusic("./sound/duelwin.mp3",true);
		  else if(mainGame->showcardcode == 2)
			  Game::PlayMusic("./sound/duellose.mp3",true);
		  else if(mainGame->dInfo.lp[LocalPlayer(0)] <= mainGame->dInfo.lp[LocalPlayer(1)]/2)
			  Game::PlayMusic("./sound/song-disadvantage.mp3",true);
		  else if(mainGame->dInfo.lp[LocalPlayer(0)] >= mainGame->dInfo.lp[LocalPlayer(1)]*2)
			  Game::PlayMusic("./sound/song-advantage.mp3",true);
		  else
			  Game::PlayMusic("./sound/song.mp3",true);
			DrawBackGround();
			DrawCards();
			DrawMisc();
			smgr->drawAll();
			driver->setMaterial(irr::video::IdentityMaterial);
			driver->clearZBuffer();
		} else if(is_building) {
			DrawDeckBd();
			Game::PlayMusic("./sound/deck.mp3",true);
		}
		else {
			Game::PlayMusic("./sound/menu.mp3",true);
		}
		DrawGUI();
		DrawSpec();
		gMutex.Unlock();
		if(signalFrame > 0) {
			signalFrame--;
			if(!signalFrame)
				frameSignal.Set();
		}
		if(waitFrame >= 0) {
			waitFrame++;
			if(waitFrame % 90 == 0) {
				stHintMsg->setText(dataManager.GetSysString(1390));
			} else if(waitFrame % 90 == 30) {
				stHintMsg->setText(dataManager.GetSysString(1391));
			} else if(waitFrame % 90 == 60) {
				stHintMsg->setText(dataManager.GetSysString(1392));
			}
		}
		driver->endScene();
		if(closeSignal.Wait(0))
			CloseDuelWindow();
		if(!device->isWindowActive())
			ignore_chain = false;
		fps++;
		cur_time = timer->getTime();
		if(cur_time < fps * 17 - 20)
#ifdef _WIN32
			Sleep(20);
#else
			usleep(20000);
#endif
		if(cur_time >= 1000) {
			myswprintf(cap, L"YGOPC | FPS: %d", fps);
			device->setWindowCaption(cap);
			fps = 0;
			cur_time -= 1000;
			timer->setTime(0);
			if(dInfo.time_player == 0 || dInfo.time_player == 1)
				if(dInfo.time_left[dInfo.time_player])
					dInfo.time_left[dInfo.time_player]--;
		}
	}
	DuelClient::StopClient(true);
	if(mainGame->dInfo.isSingleMode)
		SingleMode::StopPlay(true);
#ifdef _WIN32
	Sleep(500);
#else
	usleep(500000);
#endif
	
//	device->drop();
	engineMusic->drop(); 
}
void Game::BuildProjectionMatrix(irr::core::matrix4& mProjection, f32 left, f32 right, f32 bottom, f32 top, f32 znear, f32 zfar) {
	for(int i = 0; i < 16; ++i)
		mProjection[i] = 0;
	mProjection[0] = 2.0f * znear / (right - left);
	mProjection[5] = 2.0f * znear / (top - bottom);
	mProjection[8] = (left + right) / (left - right);
	mProjection[9] = (top + bottom) / (bottom - top);
	mProjection[10] = zfar / (zfar - znear);
	mProjection[11] = 1.0f;
	mProjection[14] = znear * zfar / (znear - zfar);
}
void Game::InitStaticText(irr::gui::IGUIStaticText* pControl, u32 cWidth, u32 cHeight, irr::gui::CGUITTFont* font, const wchar_t* text) {
	SetStaticText(pControl, cWidth, font, text);
	if(font->getDimension(dataManager.strBuffer).Height <= cHeight) {
		scrCardText->setVisible(false);
		return;
	}
	SetStaticText(pControl, cWidth-25, font, text);
	u32 fontheight = font->getDimension(L"A").Height + font->getKerningHeight();
	u32 step = (font->getDimension(dataManager.strBuffer).Height - cHeight) / fontheight + 1;
	scrCardText->setVisible(true);
	scrCardText->setMin(0);
	scrCardText->setMax(step);
	scrCardText->setPos(0);
}
void Game::SetStaticText(irr::gui::IGUIStaticText* pControl, u32 cWidth, irr::gui::CGUITTFont* font, const wchar_t* text, u32 pos) {
	int pbuffer = 0, lsnz = 0;
	u32 _width = 0, _height = 0, s = font->getCharDimension(L' ').Width;
	for(int i = 0; text[i] != 0 && i < 1023; ++i) {
		if (text[i] == L' ') {
			lsnz = i;
			if (_width + s > cWidth) {
				dataManager.strBuffer[pbuffer++] = L'\n';
				_width = 0;
				_height++;
				if(_height == pos)
					pbuffer = 0;
			}
			else {
				dataManager.strBuffer[pbuffer++] = L' ';
				_width += s;
			}
		}
		else if(text[i] == L'\n') {
			dataManager.strBuffer[pbuffer++] = L'\n';
			_width = 0;
			_height++;
			if(_height == pos)
				pbuffer = 0;
		}
		else {
			if((_width += font->getCharDimension(text[i]).Width) > cWidth) {
				dataManager.strBuffer[lsnz] = L'\n';
				_width = 0;
				for(int j = lsnz + 1; j < i; j++) {
					_width += font->getCharDimension(text[j]).Width;
				}
			}
			dataManager.strBuffer[pbuffer++] = text[i];
		}
	}
	dataManager.strBuffer[pbuffer] = 0;
	pControl->setText(dataManager.strBuffer);
}

void Game::RefreshDeck(irr::gui::IGUIComboBox* cbDeck) {
	cbDeck->clear();
#ifdef _WIN32
	WIN32_FIND_DATAW fdataw;
	HANDLE fh = FindFirstFileW(L"./decks/*.deck", &fdataw);
	if(fh == INVALID_HANDLE_VALUE)
		return;
	do {
		if(!(fdataw.dwFileAttributes & FILE_ATTRIBUTE_DIRECTORY)) {
			wchar_t* pf = fdataw.cFileName;
			while(*pf) pf++;
			while(*pf != L'.') pf--;
			*pf = 0;
			cbDeck->addItem(fdataw.cFileName);
		}
	} while(FindNextFileW(fh, &fdataw));
	FindClose(fh);
#else
	DIR * dir;
	struct dirent * dirp;
	if((dir = opendir("./decks/")) == NULL)
		return;
	while((dirp = readdir(dir)) != NULL) {
		size_t len = strlen(dirp->d_name);
		if(len < 5 || strcasecmp(dirp->d_name + len - 4, ".deck") != 0)
			continue;
		dirp->d_name[len - 4] = 0;
		wchar_t wname[256];
		BufferIO::DecodeUTF8(dirp->d_name, wname);
		cbDeck->addItem(wname);
	}
	closedir(dir);
#endif
	for(size_t i = 0; i < cbDeck->getItemCount(); ++i) {
		if(!wcscmp(cbDeck->getItem(i), gameConf.lastdeck)) {
			cbDeck->setSelected(i);
			break;
		}
	}
}
void Game::RefreshMods(){

	
	lstSinglePlayList->clear();
	WIN32_FIND_DATAW fi;
	HANDLE h = FindFirstFileW(L"./mods/*", &fi);
	if (h != INVALID_HANDLE_VALUE) {
    do {
        if (fi.dwFileAttributes & FILE_ATTRIBUTE_DIRECTORY) 
			{
			if (fi.cFileName != L"." && fi.cFileName != L"..")
			lstSinglePlayList->addItem(fi.cFileName);
			
			}
    } while (FindNextFileW(h, &fi));
    FindClose(h);

	}

	lstSinglePlayList->removeItem(0);
	//lstSinglePlayList->removeItem(1);

}


void Game::RefreshReplay() {
	lstReplayList->clear();
#ifdef _WIN32
	WIN32_FIND_DATAW fdataw;
	HANDLE fh = FindFirstFileW(L"./replay/*.rep", &fdataw);
	if(fh == INVALID_HANDLE_VALUE)
		return;
	do {
		if(!(fdataw.dwFileAttributes & FILE_ATTRIBUTE_DIRECTORY) && Replay::CheckReplay(fdataw.cFileName)) {
			lstReplayList->addItem(fdataw.cFileName);
		}
	} while(FindNextFileW(fh, &fdataw));
	FindClose(fh);
#else
	DIR * dir;
	struct dirent * dirp;
	if((dir = opendir("./replay/")) == NULL)
		return;
	while((dirp = readdir(dir)) != NULL) {
		size_t len = strlen(dirp->d_name);
		if(len < 5 || strcasecmp(dirp->d_name + len - 4, ".rep") != 0)
			continue;
		wchar_t wname[256];
		BufferIO::DecodeUTF8(dirp->d_name, wname);
		if(Replay::CheckReplay(wname))
			lstReplayList->addItem(wname);
	}
	closedir(dir);
#endif
}
void Game::RefreshSingleplay() {
	lstCPlayList->clear();
	char* ctype = new char[64];
	char* cfile = new char[64];
	LPCWSTR wfile = L"";
	wchar_t* wcfile = new wchar_t[64];

	if (cRaceC->getSelected() == 0)
		 {
			 switch (gameConf.langid)
			 {

					case 0:
						wfile = L"./scripts/levels/english/tutorial/*.lua";
				 break;
					case 1:
						wfile = L"./scripts/levels/french/tutorial/*.lua";
				 break;
					case 2:
						wfile = L"./scripts/levels/german/tutorial/*.lua";
				 break;
					case 3:
						wfile = L"./scripts/levels/italian/tutorial/*.lua";
				 break;
					case 4:
						wfile = L"./scripts/levels/portugese/tutorial/*.lua";
				 break;
					case 5:
						wfile = L"./scripts/levels/spanish/tutorial/*.lua";
				 break;

             }

		 }
	
	else if (cRaceC->getSelected() == 1)
		 {
			 switch (gameConf.langid)
			 {

					case 0:
						wfile = L"./scripts/levels/english/campaign/*.lua";
				 break;
					case 1:
						wfile = L"./scripts/levels/french/campaign/*.lua";
				 break;
					case 2:
						wfile = L"./scripts/levels/german/campaign/*.lua";
				 break;
					case 3:
						wfile = L"./scripts/levels/italian/campaign/*.lua";
				 break;
					case 4:
						wfile = L"./scripts/levels/portugese/campaign/*.lua";
				 break;
					case 5:
						wfile = L"./scripts/levels/spanish/campaign/*.lua";
				 break;

             }

		 }

	else if (cRaceC->getSelected() == 2)
		 {			 
			 wfile = L"./scripts/levels/skirmish/*.lua";
			 
		 }
	else if (cRaceC->getSelected() == 3)
		 {
			 
			 if (gameConf.enablemods)
			 {	 

			wcstombs(ctype, gameConf.smod, 10);
			strcpy(cfile, "mods/");
			strcat(cfile, ctype);
			strcat(cfile, "/scripts/levels/*.lua");
			mbstowcs(wcfile, cfile, 64);
			wfile = wcfile;
			
			}
			else
				wfile = L"./scripts/levels/*.lua";
			 

		 }

#ifdef _WIN32
	WIN32_FIND_DATAW fdataw;
	HANDLE fh = FindFirstFileW(wfile, &fdataw);
	if(fh == INVALID_HANDLE_VALUE)
		return;
	do {
		if(!(fdataw.dwFileAttributes & FILE_ATTRIBUTE_DIRECTORY))
			lstCPlayList->addItem(fdataw.cFileName);
	} while(FindNextFileW(fh, &fdataw));
	FindClose(fh);
#else
	DIR * dir;
	struct dirent * dirp;
	if((dir = opendir("./scripts/levels/")) == NULL)
		return;
	while((dirp = readdir(dir)) != NULL) {
		size_t len = strlen(dirp->d_name);
		if(len < 5 || s(dirp->d_name + len - 4, ".lua") != 0)
			continue;
		wchar_t wname[256];
		BufferIO::DecodeUTF8(dirp->d_name, wname);
		lstCPlayList->addItem(wname);
	}
	closedir(dir);
#endif
}
void Game::LoadConfig() {
	gameConf.antialias = 0;
	BufferIO::CopyWStr(L"Localhost", gameConf.servername, 30);
	BufferIO::CopyWStr(L"127.0.0.1", gameConf.serverip, 20);
	gameConf.serverport = 8911;
	gameConf.textfontsize = 12;
	BufferIO::CopyWStr(L"New Player", gameConf.nickname, 20);
	gameConf.gamename[0] = 0;
	gameConf.lastdeck[0] = 0;
	BufferIO::CopyWStr(L"textures/simhei.ttf", gameConf.textfont, 256);
	BufferIO::CopyWStr(L"textures/arialbd.ttf", gameConf.numfont, 256);
	gameConf.lastip[0] = 0;
	gameConf.lastport[0] = 0;
	gameConf.roompass[0] = 0;
	gameConf.lastreplay[0] = 0;
	gameConf.lastpuzzle[0] = 0;
	gameConf.language[0] = 0;
	gameConf.renderer[0] = 0;
	gameConf.progression[0] = 0;
	gameConf.smod[0] = 0;
	gameConf.resolution = 1;
	gameConf.langid = 0;
	gameConf.opid = 0;
	gameConf.autoplace = true;
	gameConf.randomplace = false;
	gameConf.autochain = true;
	gameConf.nodelay = false;
	gameConf.enablemusic = true;
	gameConf.musicvolume = 50.0;
	gameConf.enablesound = true;
	gameConf.soundvolume = 50.0;
	gameConf.skin_index = 0;
	gameConf.fullscreen = false;
	gameConf.enablemods = false;
	FILE* fp = fopen("data/system.conf", "r");
	if(!fp)
		return;
	char linebuf[256];
	char strbuf[32];
	char valbuf[256];
	wchar_t wstr[256];
	fseek(fp, 0, SEEK_END);
	int fsize = ftell(fp);
	fseek(fp, 0, SEEK_SET);
	while(ftell(fp) < fsize) {
		fgets(linebuf, 250, fp);
		sscanf(linebuf, "%s = %99[^\n]", strbuf, valbuf);
		if(!strcmp(strbuf, "antialias")) {
			gameConf.antialias = atoi(valbuf);
		} else if(!strcmp(strbuf, "use_d3d")) {
			gameConf.use_d3d = atoi(valbuf) > 0;
		} else if(!strcmp(strbuf, "enable_graphics")) {
			gameConf.enablegraphics = atoi(valbuf) > 0;		
		} else if(!strcmp(strbuf, "errorlog")) {
			enable_log = atoi(valbuf);
		} else if(!strcmp(strbuf, "nickname")) {
			BufferIO::DecodeUTF8(valbuf, wstr);
			BufferIO::CopyWStr(wstr, gameConf.nickname, 20);
		} else if(!strcmp(strbuf, "gamename")) {
			BufferIO::DecodeUTF8(valbuf, wstr);
			BufferIO::CopyWStr(wstr, gameConf.gamename, 20);
		} else if(!strcmp(strbuf, "lastdeck")) {
			BufferIO::DecodeUTF8(valbuf, wstr);
			BufferIO::CopyWStr(wstr, gameConf.lastdeck, 64);
		} else if(!strcmp(strbuf,"language")) {
			BufferIO::DecodeUTF8(valbuf, wstr);
			BufferIO::CopyWStr(wstr, gameConf.language, 10);
		} else if(!strcmp(strbuf,"lang_id")) {
			gameConf.langid = atoi(valbuf);
		} else if(!strcmp(strbuf,"enable_mods")) {
			gameConf.enablemods = atoi(valbuf) > 0;
		} else if(!strcmp(strbuf,"mod")) {
			BufferIO::DecodeUTF8(valbuf, wstr);
			BufferIO::CopyWStr(wstr, gameConf.smod, 10);
		} else if(!strcmp(strbuf, "textfont")) {
			sscanf(linebuf, "%s = %s", strbuf, valbuf);
			BufferIO::DecodeUTF8(valbuf, wstr);
			int textfontsize;
			sscanf(linebuf, "%s = %s %d", strbuf, valbuf, &textfontsize);
			gameConf.textfontsize = textfontsize;
			BufferIO::CopyWStr(wstr, gameConf.textfont, 256);
		} else if(!strcmp(strbuf, "numfont")) {
			sscanf(linebuf, "%s = %s", strbuf, valbuf);
			BufferIO::DecodeUTF8(valbuf, wstr);
			BufferIO::CopyWStr(wstr, gameConf.numfont, 256);
		} else if(!strcmp(strbuf,"servername")) {
			BufferIO::DecodeUTF8(valbuf, wstr);
			BufferIO::CopyWStr(wstr, gameConf.servername, 30);
		} else if(!strcmp(strbuf,"serverip")) {
			BufferIO::DecodeUTF8(valbuf, wstr);
			BufferIO::CopyWStr(wstr, gameConf.lastip, 20);
		} else if(!strcmp(strbuf, "serverport")) {
			BufferIO::DecodeUTF8(valbuf, wstr);
			BufferIO::CopyWStr(wstr, gameConf.lastport, 10);
		} else if(!strcmp(strbuf, "roompass")) {
			BufferIO::DecodeUTF8(valbuf, wstr);
			BufferIO::CopyWStr(wstr, gameConf.roompass, 30);
		} else if(!strcmp(strbuf,"auto_card_placing")) {
			gameConf.autoplace = atoi(valbuf) > 0;
		} else if(!strcmp(strbuf,"random_card_placing")) {
			gameConf.randomplace = atoi(valbuf) > 0;
		} else if(!strcmp(strbuf,"auto_chain_order")) {
			gameConf.autochain = atoi(valbuf) > 0;
		} else if(!strcmp(strbuf,"no_delay_for_chain")) {
			gameConf.nodelay = atoi(valbuf) > 0;
		} else if(!strcmp(strbuf,"skin_index")) {
			gameConf.skin_index = atoi(valbuf) > 0;
		} else if(!strcmp(strbuf,"fullscreen")) {
			gameConf.fullscreen = atoi(valbuf) > 0;
		} else if(!strcmp(strbuf,"resolution")) {
			gameConf.resolution = atoi(valbuf) > 0;
		} else if(!strcmp(strbuf,"enable_music")) {
			gameConf.enablemusic = atoi(valbuf) > 0;
		} else if(!strcmp(strbuf,"music_volume")) {
			gameConf.musicvolume = atof(valbuf);
		} else if(!strcmp(strbuf,"enable_sound")) {
			gameConf.enablesound = atoi(valbuf) > 0;
		} else if(!strcmp(strbuf,"sound_volume")) {
			gameConf.soundvolume = atof(valbuf);
		} else if(!strcmp(strbuf,"lastpuzzle")) {
			BufferIO::DecodeUTF8(valbuf, wstr);
			BufferIO::CopyWStr(wstr, gameConf.lastpuzzle, 256);
		} else if(!strcmp(strbuf,"lastreplay")) {
			BufferIO::DecodeUTF8(valbuf, wstr);
			BufferIO::CopyWStr(wstr, gameConf.lastreplay, 256);		 	
		} 
		
	}
	fclose(fp);


}
void Game::SaveConfig() {
	FILE* fp = fopen("data/system.conf", "w");
	fprintf(fp, "#config file\n");
	char linebuf[256];
	fprintf(fp, "use_d3d = %d\n", gameConf.use_d3d ? 1 : 0);
	fprintf(fp, "antialias = %d\n", gameConf.antialias);
	fprintf(fp, "enable_graphics = %d\n", gameConf.enablegraphics ? 1:0);
	fprintf(fp, "errorlog = %d\n", enable_log);
	fprintf(fp, "skin_index = %d\n", gameConf.skin_index);
	BufferIO::CopyWStr(ebNickName->getText(), gameConf.nickname, 20);
	BufferIO::EncodeUTF8(gameConf.nickname, linebuf);
	fprintf(fp, "nickname = %s\n", linebuf);
	BufferIO::EncodeUTF8(gameConf.gamename, linebuf);
	fprintf(fp, "gamename = %s\n", linebuf);
	BufferIO::EncodeUTF8(gameConf.lastdeck, linebuf);
	fprintf(fp, "lastdeck = %s\n", linebuf);
	BufferIO::EncodeUTF8(gameConf.language, linebuf);
	fprintf(fp, "language = %s\n", linebuf);
	fprintf(fp, "lang_id = %d\n", gameConf.langid);
	fprintf(fp, "enable_mods = %d\n", gameConf.enablemods ? 1:0);
	BufferIO::EncodeUTF8(gameConf.smod, linebuf);
	fprintf(fp, "mod = %s\n", linebuf);
	BufferIO::EncodeUTF8(gameConf.textfont, linebuf);
	fprintf(fp, "textfont = %s %d\n", linebuf, gameConf.textfontsize);
	BufferIO::EncodeUTF8(gameConf.numfont, linebuf);
	fprintf(fp, "numfont = %s\n", linebuf);
	fprintf(fp, "resolution = %d\n", gameConf.resolution);
	fprintf(fp, "fullscreen = %d\n", gameConf.fullscreen);
	BufferIO::EncodeUTF8(gameConf.lastip, linebuf);
	fprintf(fp, "serverip = %s\n", linebuf);
	BufferIO::EncodeUTF8(gameConf.lastport, linebuf);
	fprintf(fp, "serverport = %s\n", linebuf);
	fprintf(fp, "sound_volume = %f\n", gameConf.soundvolume);
	fprintf(fp, "music_volume = %f\n", gameConf.musicvolume);
	fclose(fp);
}
void Game::RefreshConfig(){

	if (gameConf.langid == 0)
		cbLanguageSelect->setSelected(0);
	else if (gameConf.langid == 1)
		cbLanguageSelect->setSelected(1);
	else if (gameConf.langid == 2)
		cbLanguageSelect->setSelected(2);
	else if (gameConf.langid == 3)
		cbLanguageSelect->setSelected(3);
	else if (gameConf.langid = 4)
		cbLanguageSelect->setSelected(4);
	else if (gameConf.langid = 5)
		cbLanguageSelect->setSelected(5);

	if (gameConf.use_d3d == true)
		cbRenderSelect->setSelected(0);
	else if (gameConf.use_d3d == false)
		cbRenderSelect->setSelected(1);

	if (gameConf.skin_index == 0)
		cbSkinSelect->setSelected(0);
	else if (gameConf.skin_index == 1)
		cbSkinSelect->setSelected(1);
	
	if (gameConf.enablegraphics == true)
		chkEnableAdvanceGraphics->setChecked(1);
	else if (gameConf.enablegraphics == false)
		chkEnableAdvanceGraphics->setChecked(0);

	if (gameConf.enablemods == true)
		chkEnableMods->setChecked(1);
	else if (gameConf.enablemods == false)
		chkEnableMods->setChecked(0);
}
void Game::LoadProgress() {

	gameConf.progress = 0;
	gameConf.tutorial = 0;
	gameConf.score1 = 0;
	gameConf.score2 = 0;
	gameConf.score3 = 0;
	gameConf.score4 = 0;
	gameConf.score5 = 0;
	gameConf.score6 = 0;


	FILE* fp = fopen("data/save.conf", "r");
	if (!fp)
		return;
	char linebuf[256];
	char strbuf[32];
	char valbuf[256];
	fseek(fp, 0, SEEK_END);
	int fsize = ftell(fp);
	fseek(fp, 0, SEEK_SET);
	while (ftell(fp) < fsize) {
		fgets(linebuf, 250, fp);
		sscanf(linebuf, "%s = %99[^\n]", strbuf, valbuf);
		if (!strcmp(strbuf, "progress")) {
			gameConf.progress = atoi(valbuf);
		}
		else if (!strcmp(strbuf, "tutorial")) {
			gameConf.tutorial = atoi(valbuf);
		}
		else if (!strcmp(strbuf, "score_greek")) {
			gameConf.score1 = atoi(valbuf);
		}
		else if (!strcmp(strbuf, "score_egypt")) {
			gameConf.score2 = atoi(valbuf);
		}
		else if (!strcmp(strbuf, "score_persia")) {
			gameConf.score3 = atoi(valbuf);
		}
		else if (!strcmp(strbuf, "score_celtic")) {
			gameConf.score4 = atoi(valbuf);
		}
		else if (!strcmp(strbuf, "score_babylon")) {
			gameConf.score5 = atoi(valbuf);
		}
		else if (!strcmp(strbuf, "score_norse")) {
			gameConf.score6 = atoi(valbuf);
		}

	}
	fclose(fp);


}
void Game::SaveProgress(){
		
	if (gameConf.progress > -1)
		swprintf(gameConf.progression, 10, L"%d", gameConf.progress);

	if (gameConf.progression != L"")
	stProgressC->setText(gameConf.progression);

	FILE* fp = fopen("data/save.conf", "w");
	fprintf(fp, "#save file\n");
	fprintf(fp, "progress = %d\n", gameConf.progress);
	fprintf(fp, "tutorial = %d\n", gameConf.tutorial);
	fprintf(fp, "score_greek = %d\n", gameConf.score1);
	fprintf(fp, "score_egypt = %d\n", gameConf.score2);
	fprintf(fp, "score_persia = %d\n", gameConf.score3);
	fprintf(fp, "score_celtic = %d\n", gameConf.score4);
	fprintf(fp, "score_babylon = %d\n", gameConf.score5);
	fprintf(fp, "score_norse = %d\n", gameConf.score6);
	fclose(fp);

}
void Game::PlayMusic(char* song, bool loop){
	if(gameConf.enablemusic){
		if(!engineMusic->isCurrentlyPlaying(song)){
					engineMusic->stopAllSounds();
					engineMusic->play2D(song, loop);
					engineMusic->setSoundVolume(gameConf.musicvolume);
		}
	}
}
void Game::PlaySound(char* sound){
	if(gameConf.enablesound){
		engineSound->play2D(sound);
		engineSound->setSoundVolume(gameConf.soundvolume);
	}
}
void Game::ShowCardInfo(int code) {
	CardData cd;
	wchar_t formatBuffer[256];
	if(!dataManager.GetData(code, &cd))
		memset(&cd, 0, sizeof(CardData));
	imgCard->setImage(imageManager.GetTexture(code));
	imgCard->setScaleImage(true);
	if(cd.alias != 0 && (cd.alias - code < 10 || code - cd.alias < 10))
		myswprintf(formatBuffer, L"%ls[%08d]", dataManager.GetName(cd.alias), cd.alias);
	else myswprintf(formatBuffer, L"%ls[%08d]", dataManager.GetName(code), code);
	stName->setText(formatBuffer);
	if(cd.type & TYPE_MONSTER) {
		myswprintf(formatBuffer, L"[%ls] %ls/%ls", dataManager.FormatType(cd.type), dataManager.FormatRace(cd.race), dataManager.FormatAttribute(cd.attribute));
		stInfo->setText(formatBuffer);
		formatBuffer[0] = L'[';
		for(unsigned int i = 1; i <= cd.level; ++i)
			formatBuffer[i] = 0x2605;
		formatBuffer[cd.level + 1] = L']';
		formatBuffer[cd.level + 2] = L' ';
		if(cd.attack < 0 && cd.defence < 0)
			myswprintf(&formatBuffer[cd.level + 3], L"?/?");
		else if(cd.attack < 0)
			myswprintf(&formatBuffer[cd.level + 3], L"?/%d", cd.defence);
		else if(cd.defence < 0)
			myswprintf(&formatBuffer[cd.level + 3], L"%d/?", cd.attack);
		else
			myswprintf(&formatBuffer[cd.level + 3], L"%d/%d", cd.attack, cd.defence);	
		stDataInfo->setText(formatBuffer);
	} else {
		myswprintf(formatBuffer, L"[%ls]", dataManager.FormatType(cd.type));
		stInfo->setText(formatBuffer);
		stDataInfo->setText(L"");
	}
	showingtext = dataManager.GetText(code);
	const auto& tsize = stText->getRelativePosition();
	InitStaticText(stText, tsize.getWidth() - 30, tsize.getHeight(), textFont, showingtext);
}
void Game::AddChatMsg(wchar_t* msg, int player) {
	for(int i = 7; i > 0; --i) {
		chatMsg[i] = chatMsg[i - 1];
		chatTiming[i] = chatTiming[i - 1];
		chatType[i] = chatType[i - 1];
	}
	chatMsg[0].clear();
	chatTiming[0] = 1200;
	chatType[0] = player;
	switch(player) {
	case 0: //from host
		chatMsg[0].append(dInfo.hostname);
		chatMsg[0].append(L": ");
		break;
	case 1: //from client
		chatMsg[0].append(dInfo.clientname);
		chatMsg[0].append(L": ");
		break;
	case 2: //host tag
		chatMsg[0].append(dInfo.hostname_tag);
		chatMsg[0].append(L": ");
		break;
	case 3: //client tag
		chatMsg[0].append(dInfo.clientname_tag);
		chatMsg[0].append(L": ");
		break;
	case 7: //local name
		chatMsg[0].append(mainGame->gameConf.nickname);
		chatMsg[0].append(L": ");
		break;
	case 8: //system custom message, no prefix.
		chatMsg[0].append(L"[System]: ");
		break;
	case 9: //error message
		chatMsg[0].append(L"[Script error:] ");
		break;
	default: //from watcher or unknown
		if(player < 11 || player > 19)
			chatMsg[0].append(L"[---]: ");
	}
	chatMsg[0].append(msg);
}
void Game::ClearTextures() {
	matManager.mCard.setTexture(0, 0);
	mainGame->imgCard->setImage(0);
	mainGame->btnPSAU->setImage();
	mainGame->btnPSDU->setImage();
	mainGame->btnCardSelect[0]->setImage();
	mainGame->btnCardSelect[1]->setImage();
	mainGame->btnCardSelect[2]->setImage();
	mainGame->btnCardSelect[3]->setImage();
	mainGame->btnCardSelect[4]->setImage();
	imageManager.ClearTexture();
}
void Game::CloseDuelWindow() {
	for(auto wit = fadingList.begin(); wit != fadingList.end(); ++wit) {
		if(wit->isFadein)
			wit->autoFadeoutFrame = 1;
	}
	wACMessage->setVisible(false);
	wANAttribute->setVisible(false);
	wANCard->setVisible(false);
	wANNumber->setVisible(false);
	wANRace->setVisible(false);
	wCardImg->setVisible(false);
	wCardSelect->setVisible(false);
	wCmdMenu->setVisible(false);
	wFTSelect->setVisible(false);
	wHand->setVisible(false);
	wInfos->setVisible(false);
	wMessage->setVisible(false);
	wOptions->setVisible(false);
	wPhase->setVisible(false);
	wPosSelect->setVisible(false);
	wQuery->setVisible(false);
	wReplayControl->setVisible(false);
	wReplaySave->setVisible(false);
	stHintMsg->setVisible(false);
	btnSideOK->setVisible(false);
	btnLeaveGame->setVisible(false);
	wChat->setVisible(false);
	lstLog->clear();
	logParam.clear();
	lstHostList->clear();
	DuelClient::hosts.clear();
	ClearTextures();
	closeDoneSignal.Set();
}
int Game::LocalPlayer(int player) {
	return dInfo.isFirst ? player : 1 - player;
}
const wchar_t* Game::LocalName(int local_player) {
	return local_player == 0 ? dInfo.hostname : dInfo.clientname;
}
void Game::OnResize()
{
	wMainMenu->setRelativePosition(ResizeWin(370, 200, 650, 450));
	wCampaignMenu->setRelativePosition(ResizeWin(220, 100, 800, 520));
	wLanWindow->setRelativePosition(ResizeWin(220, 100, 800, 520));
	wCreateHost->setRelativePosition(ResizeWin(320, 100, 700, 520));
	wHostPrepare->setRelativePosition(ResizeWin(270, 120, 750, 440));
	wReplay->setRelativePosition(ResizeWin(220, 100, 800, 520));
	wSinglePlay->setRelativePosition(ResizeWin(220, 100, 800, 520));
	wHand->setRelativePosition(ResizeWin(500, 450, 825, 605));
	wFTSelect->setRelativePosition(ResizeWin(550, 240, 780, 340));
	wMessage->setRelativePosition(ResizeWin(490, 200, 840, 340));
	wACMessage->setRelativePosition(ResizeWin(490, 240, 840, 300));
	wQuery->setRelativePosition(ResizeWin(490, 200, 840, 340));
	wOptions->setRelativePosition(ResizeWin(490, 200, 840, 340));
	wPosSelect->setRelativePosition(ResizeWin(340, 200, 935, 410));
	wCardSelect->setRelativePosition(ResizeWin(320, 100, 1000, 400));
	wANNumber->setRelativePosition(ResizeWin(550, 200, 780, 295));
	wANCard->setRelativePosition(ResizeWin(560, 170, 770, 370));
	wANAttribute->setRelativePosition(ResizeWin(500, 200, 830, 285));
	wANRace->setRelativePosition(ResizeWin(480, 200, 850, 385));
	wReplaySave->setRelativePosition(ResizeWin(510, 200, 820, 320));
	stHintMsg->setRelativePosition(ResizeWin(500, 60, 820, 90));

	wChat->setRelativePosition(ResizeWin(305, 615, 1020, 640, true));
	ebChatInput->setRelativePosition(recti(3, 2, window_size.Width - wChat->getRelativePosition().UpperLeftCorner.X - 6, 22));


	
	wCardImg->setRelativePosition(Resize(1, 1, 199, 273));
	imgCard->setRelativePosition(Resize(9, 9, 187, 262));
	wInfos->setRelativePosition(Resize(1, 275, 301, 639));
	stName->setRelativePosition(recti(10, 10, 287 * window_size.Width / 1024, 32));
	stInfo->setRelativePosition(recti(15, 37, 296 * window_size.Width / 1024, 60));
	stDataInfo->setRelativePosition(recti(15, 60, 296 * window_size.Width / 1024, 83));
	stText->setRelativePosition(recti(15, 83, 296 * window_size.Width / 1024 - 30, 324 * window_size.Height / 640));
	scrCardText->setRelativePosition(recti(stInfo->getRelativePosition().getWidth() - 20, 83, stInfo->getRelativePosition().getWidth(), 324 * window_size.Height / 640));
	lstLog->setRelativePosition(Resize(10, 10, 290, 290));
	btnClearLog->setRelativePosition(Resize(160, 300, 260, 325));


	btnLeaveGame->setRelativePosition(Resize(205, 5, 295, 80));
	wReplayControl->setRelativePosition(Resize(205, 143, 295, 273));
	btnReplayStart->setRelativePosition(Resize(5, 5, 85, 25));
	btnReplayPause->setRelativePosition(Resize(5, 30, 85, 50));
	btnReplayStep->setRelativePosition(Resize(5, 55, 85, 75));
	btnReplaySwap->setRelativePosition(Resize(5, 80, 85, 100));
	btnReplayExit->setRelativePosition(Resize(5, 105, 85, 125));
	
	scrFilter->setRelativePosition(Resize(999, 161, 1019, 629));

	wPhase->setRelativePosition(Resize(480, 310, 855, 330));
	btnDP->setRelativePosition(Resize(0, 0, 50, 20));
	btnSP->setRelativePosition(Resize(65, 0, 115, 20));
	btnM1->setRelativePosition(Resize(130, 0, 180, 20));
	btnBP->setRelativePosition(Resize(195, 0, 245, 20));
	btnM2->setRelativePosition(Resize(260, 0, 310, 20));
	btnEP->setRelativePosition(Resize(325, 0, 375, 20));
	
	wDeckEdit->setRelativePosition(Resize(309, 8, 605, 130));
	cbDBLFList->setRelativePosition(Resize(80, 5, 220, 30));
	cbDBDecks->setRelativePosition(Resize(80, 35, 220, 60));
	btnClearDeck->setRelativePosition(Resize(240, 95, 290, 116));
	btnSortDeck->setRelativePosition(Resize(185, 95, 235, 116));
	btnShuffleDeck->setRelativePosition(Resize(130, 95, 180, 116));
	btnSaveDeck->setRelativePosition(Resize(225, 35, 290, 60));
	btnSaveDeckAs->setRelativePosition(Resize(225, 65, 290, 90));
	btnDBExit->setRelativePosition(Resize(10, 95, 90, 116));
	ebDeckname->setRelativePosition(Resize(80, 65, 220, 90));
	wFilter->setRelativePosition(Resize(610, 8, 1020, 130));
	
	cbCardType->setRelativePosition(Resize(60, 3, 130, 23));
	cbCardType2->setRelativePosition(Resize(130, 3, 190, 23));
	cbRace->setRelativePosition(Resize(60, 49, 190, 69));
	cbAttribute->setRelativePosition(Resize(60, 26, 190, 46));
	cbLimit->setRelativePosition(Resize(260, 3, 340, 23));
	ebStar->setRelativePosition(Resize(60, 72, 140, 92));
	ebAttack->setRelativePosition(Resize(260, 26, 340, 46));
	ebDefence->setRelativePosition(Resize(260, 49, 340, 69));
	ebCardName->setRelativePosition(Resize(260, 72, 390, 92));
	btnStartFilter->setRelativePosition(Resize(260, 96, 390, 118));
	btnClearFilter->setRelativePosition(Resize(205, 96, 255, 118));

	stLabel1->setRelativePosition(Resize(10, 9, 100, 29));
	stLabel2->setRelativePosition(Resize(10, 39, 100, 59));
	stLabel3->setRelativePosition(Resize(10, 5, 70, 25));
	stLabel4->setRelativePosition(Resize(205, 5, 280, 25));
	stLabel5->setRelativePosition(Resize(10, 28, 70, 48));
	stLabel6->setRelativePosition(Resize(10, 51, 70, 71));
	stLabel7->setRelativePosition(Resize(205, 28, 280, 48));
	stLabel8->setRelativePosition(Resize(205, 51, 280, 71));
	stLabel9->setRelativePosition(Resize(10, 74, 80, 94));
	stLabel10->setRelativePosition(Resize(205, 74, 280, 94));

	
	btnSideOK->setRelativePosition(Resize(510, 40, 820, 80));
	btnDeleteDeck->setRelativePosition(Resize(10, 68, 75,89));
}
recti Game::Resize(s32 x, s32 y, s32 x2, s32 y2)
{
	x = x * window_size.Width / 1024;
	y = y * window_size.Height / 640;
	x2 = x2 * window_size.Width / 1024;
	y2 = y2 * window_size.Height / 640;
	return recti(x, y, x2, y2);
}
recti Game::Resize(s32 x, s32 y, s32 x2, s32 y2, s32 dx, s32 dy, s32 dx2, s32 dy2)
{
	x = x * window_size.Width / 1024 + dx;
	y = y * window_size.Height / 640 + dy;
	x2 = x2 * window_size.Width / 1024 + dx2;
	y2 = y2 * window_size.Height / 640 + dy2;
	return recti(x, y, x2, y2);
}
position2di Game::Resize(s32 x, s32 y, bool reverse)
{
	if (reverse)
	{
		x = x * 1024 / window_size.Width;
		y = y * 640 / window_size.Height;
	}
	else
	{
		x = x * window_size.Width / 1024;
		y = y * window_size.Height / 640;
	}
	return position2di(x, y);
}
recti Game::ResizeWin(s32 x, s32 y, s32 x2, s32 y2, bool chat)
{
	s32 sx = x2 - x;
	s32 sy = y2 - y;
	if (chat)
	{
		y = window_size.Height - sy;
		x2 = window_size.Width;
		y2 = y + sy;
		return recti(x, y, x2, y2);
	}


	x = (x + sx / 2) * window_size.Width / 1024 - sx / 2;
	y = (y + sy / 2) * window_size.Height / 640 - sy / 2;
	x2 = sx + x;
	y2 = sy + y;
	return recti(x, y, x2, y2);
}
recti Game::ResizeElem(s32 x, s32 y, s32 x2, s32 y2)
{
	s32 sx = x2 - x;
	s32 sy = y2 - y;
	x = (x + sx / 2 - 100) * window_size.Width / 1024 - sx / 2 + 100;
	y = y * window_size.Height / 640;
	x2 = sx + x;
	y2 = sy + y;
	return recti(x, y, x2, y2);
}

}
