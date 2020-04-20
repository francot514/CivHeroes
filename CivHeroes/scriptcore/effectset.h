
#ifndef EFFECTSET_H_
#define EFFECTSET_H_

#include <stdlib.h>
#include <array>
#include <vector>
#include <algorithm>

class effect;

bool effect_sort_id(const effect* e1, const effect* e2);

struct effect_set {
	effect_set(): count(0) {}
	void add_item(effect* peffect) {
		if(count >= 64) return;
		container[count++] = peffect;
	}
	void remove_item(int index) {
		if(index >= count)
			return;
		if(index == count - 1) {
			count--;
			return;
		}
		for(int i = index; i < count - 1; ++i)
			container[i] = container[i + 1];
		count--;
	}
	void clear() {
		count = 0;
	}
	void sort() {
		if(count < 2)
			return;
		std::sort(container.begin(), container.begin() + count, effect_sort_id);
	}
	effect*& get_last() {
		return container[count - 1];
	}
	effect*& operator[] (int index) {
		return container[index];
	}
	effect*& at(int index) {
		return container[index];
	}
	std::array<effect*, 64> container;
	int count;
};

struct effect_set_v {
	effect_set_v(): count(0) {}
	void add_item(effect* peffect) {
		container.push_back(peffect);
		count++;
	}
	void remove_item(int index) {
		if(index >= count)
			return;
		container.erase(container.begin() + index);
		count--;
	}
	void clear() {
		container.clear();
		count = 0;
	}
	void sort() {
		if(count < 2)
			return;
		std::sort(container.begin(), container.begin() + count, effect_sort_id);
	}
	effect*& get_last() {
		return container[count - 1];
	}
	effect*& operator[] (int index) {
		return container[index];
	}
	effect*& at(int index) {
		return container[index];
	}
	std::vector<effect*> container;
	int count;
};

#endif //EFFECTSET_H_
