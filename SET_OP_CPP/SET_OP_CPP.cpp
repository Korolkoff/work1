// SET_OP_CPP.cpp: определяет точку входа для консольного приложения.
//


#include "stdafx.h"
#include <iostream>
#include <string>
#include <vector>
#include <algorithm> 
#include <functional> 
#include <cctype>
#include <locale>
#include <list>
#include <string>
#include <cstdlib>
#include <typeinfo>
//#include <System.dll>

#define FLOAT_VALUE 99999
//#define infinity -999999999
using namespace std;
/*using namespace System;
using namespace System::Collections;
using namespace System::Globalization;*/

struct _operSet
{
	string given;
	string sX, sY;
	double fX , fY;
	unsigned char tX, tY;	bool isNeg;
	//флажок, метка на удаление
	bool needDel;

	_operSet::_operSet(string pgiven, string psX, string psY, double pfX, double pfY, unsigned char ptX, unsigned char ptY, bool pisNeg)
	//	: this() // вызываем “конструктор по умолчанию”
	{
		given = pgiven;
		sX = psX;
		sY = psY;
		fX = pfX;
		fY = pfY;
		tX = ptX;
		tY = ptY;
		isNeg = pisNeg;
		needDel = false;
		// поле needDel инициализировано неявно!
	}
	//конструктор по умолчанию
	_operSet::_operSet()
	{
		//не работает код ниже
		//given = NULL;
		given = "";
		sX = "";
		sY = "";
		tX = '(';
		tY = ')';
		fX = FLOAT_VALUE;//будем считать что это минимальное/max дабл число
		fY = FLOAT_VALUE;
		isNeg = false;
		needDel = false;
	}
};

//прототипы:
vector<_operSet> convertToNormalList(vector<_operSet>* set);
vector<_operSet> sortFromNumberToString(vector<_operSet>* set);
void showVector(vector<_operSet> a);
int compare_str(const void * x1, const void * x2);
int compare_double(const void * x1, const void * x2);
int compare_bool(const void * x1, const void * x2);
//конец прототипов

int main(int argc, char* argv[])
{

	int b2b;
	// std::cin >> b2b;

	 //gjxtve nj yt hf,jnftn
	// _operSet entity1 = new _operSet("апра","авп","вапв",4,4,'(',')',false);
	
	 //NULL не подходит в качестве параметра для строкового типа
	 _operSet entity0 = _operSet("", "", "", 1, 8, '[', ']', false);
	 _operSet entity1 = _operSet("", "", "", 3, 5, '(', ')', false);
	 _operSet entity2 = _operSet("aaa...abc", "aaa", "eee", FLOAT_VALUE, FLOAT_VALUE, '[', ']', false);
	 _operSet entity3 = _operSet("abb...abc", "abb", "ddd", FLOAT_VALUE, FLOAT_VALUE, '(', ')', false);
	 _operSet entity4 = _operSet("abb...abc", "abc", "kkk", FLOAT_VALUE, FLOAT_VALUE, '(', ')', false);
	 _operSet entity5 = _operSet("abb...abc", "mmm", "zzz", FLOAT_VALUE, FLOAT_VALUE, '(', ')', false);
	 _operSet entity6 = _operSet("beauty", "lol", "yolo", FLOAT_VALUE, FLOAT_VALUE, '(', ']',true);
	 _operSet entity7;
	 //vector<_operSet> set = new vector<_operSet>();
	 vector<_operSet> set;
	 set.push_back(entity0);
	 set.push_back(entity1);
	 set.push_back(entity2);
	 set.push_back(entity3);
	 set.push_back(entity4);
	 set.push_back(entity5);
	 set.push_back(entity6);
	/* 
	 if( "aaa">"abc") 
	 string str1 = "aaa";
	 string str2 = "abc";
		if (strcmp("aaa", "abc"))
		if(str1.compare(str2))
	 cout <<"true";
		*/

	 set = sortFromNumberToString( &set);	 
	 // работает
	// set = convertToNormalList(&set);
	 showVector(set);

	 std::cin >>b2b;
 return 0;
}
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	

/*
list<_operSet> convertToNormalList(list <_operSet> set)
{
	int i = 0;
	_operSet first;
	_operSet second;
	for (auto entity : set)
	{
	}
}
*/

//делаем частично-упорядоченное множество из множества
vector<_operSet> sortFromNumberToString(vector<_operSet>* set)
	{
		vector<_operSet>f;
		vector<_operSet>s;
		vector<_operSet>b;
		for (auto v : *set) 
		{
			//можно дописать обработчик на знак "!"
			//убедиться что не будет зацикливания, обычно есть контракт на то, что пройдется по каждому элементу один раз
			if (v.isNeg == true)
			{
				//if ((&set->at(i))->isNeg == true)
				b.push_back(v);
				//a.erase(v);
			}
			else if (v.sX == "")
			{
				f.push_back(v);
			}
			else
			{
				s.push_back(v);
			}
		}
		//std::sort(f.begin(), f.end(), (*comparator)compare_str);

		qsort(&s, s.size(), sizeof(_operSet), compare_str);
		qsort(&f, f.size(), sizeof(_operSet), compare_double);
		qsort(&b, b.size(), sizeof(_operSet), compare_bool);


		//*set = f;
		f.insert(f.end(), s.begin(), s.end());
		f.insert(f.end(), b.begin(), b.end());
		
		return f;
		
/*

for(vector<int>::iterator iter=val.begin();iter!=val.end();iter++){
cout<<*iter<<" ";
}

for(vector<int>::size_type i=0;i!=val.size();i++){
cout<<val[i]<<" ";
}

int f(int v){
cout<<v<<" ";
}
for_each(val.begin(),val.end(),f);

*/
}

vector<_operSet> convertToNormalList(vector<_operSet>* set)
{
	//vector <_operSet> a = new vector()
	//ругается на разыменовывание указателя
	vector <_operSet> a = *set;
	int size = sizeof(_operSet);
	//vector <_operSet>::iterator Iter;
//	CompareOptions myOptions = CompareOptions.None;
	for (int i = 0; i < a.size() - 1; i++)
	{
		//if (    (&set->at(i) )->isNeg == true)
		if (a[i].isNeg == true|| a[i+1].isNeg == true)
		{
			//если планируется обработка отрицания то писать здесь
		}
		//проверяем, что два соседних элемента не являются различнымм по типу(числа и строки)
		else if (a[i].fX != FLOAT_VALUE && a[i + 1].sX != "")
		{
			//i++;
			//cout << "rebro " << i;
		}
		//работа с числовыми множествами1 - случай пересечения
		else if (a[i + 1].fX < a[i].fY && a[i+1].sX==""&& a[i].sX=="")
		{
			a[i + 1].fX = a[i].fX;
			a[i + 1].tX = a[i].tX;
			//проверка на верхнюю границу множеств
			if (a[i].fY > a[i+1].fY)
			{
				a[i + 1].fY = a[i].fY;
				a[i + 1].tY = a[i].tY;
			}
			//a.erase(a[i]);
			a[i].needDel = true;
		}
		//работа с числовыми множествами2  - случай границы
		else if ((a[i + 1].fX == a[i].fY) && !((a[i].tY == ')')      && (a[i + 1].tX == '(')) && (a[i + 1].sX == "") && (a[i].sX == ""))
		{
			a[i + 1].fX = a[i].fX;
			a[i + 1].tX = a[i].tX;
			//a.erase(a[i]);
			a[i].needDel = true;
		}
		//работа со строками1 - случай пересечения
		//тупо сравниваем строки, как сравниваются - без понятия
		else if (a[i].sY.compare(a[i + 1].sX)>0)
		{
			a[i + 1].sX = a[i].sX;
			a[i + 1].tX = a[i].tX;
			////проверка на верхнюю границу множеств
			if (a[i].sY.compare(a[i + 1].sY)>0)
			{
				a[i + 1].sY = a[i].sY;
				a[i + 1].tY = a[i].tY;
			}

			//a.erase();
		//	 Iter = a[i];
		//	a.erase(a[i]);
			a[i].needDel = true;
		}
		//работа со строками2 - случай границы
		else if (0==a[i].sY.compare(a[i + 1].sX) && !((a[i].tY == ')') && (a[i + 1].tX == '('))    )
		{
			a[i + 1].sX = a[i].sX;
			a[i + 1].tX = a[i].tX;
			//a.erase(a[i]);
			a[i].needDel = true;

		}
		else {};
		//cout << i<<endl;

	}
	return a;
}


//вывод множества построчный
void showVector(vector<_operSet> a)
{
	for (auto v : a) {
		//вывод без пометки на удаление
		if (v.needDel == false) 
		{
			std::cout << (int)v.fX << " " << (int)v.fY << " " << v.sX << " " << v.sY << " "<<v.tX << " " << v.tY << endl;
			//a.erase(v);
		}
	}
}
/*
bool chooseUpBound(double a, double b)
{
	if (a>b)
}
bool chooseUpBound(string a, string b)
{

}

list<_operSet> convertToNormalList(list <_operSet> set)
{
	for (auto i : set)
	{
		if (i.isNeg == 0)
		{

		}
		else
		{
			//if ()
		}
	}
}
*/
int compare_str(const void * x1, const void * x2)   // функция сравнения элементов массива
{

	_operSet a = const_cast <_operSet*>(x1);
	_operSet a1 = static_cast <_operSet*>(x1);
	_operSet a2 = dynamic_cast <_operSet*>(x1);
	_operSet a3 = reinterpret_cast <_operSet*>(x1);
	_operSet a4 = *(_operSet*)(x1);
	
	//if (typeid(&x1).name()=="double")
	return 0;// (x1->sX).compare(x2->sX);              // если результат вычитания равен 0, то числа равны, < 0: x1 < x2; > 0: x1 > x2
}

int compare_double(const void * x1, const void * x2)
{
	return 0;// x1->fX - x2->fX;
}
/*
int compare_double(_operSet * x1, _operSet * x2)
{
	return x1->fX - x2->fX;
}*/
int compare_bool(const void * x1, const void * x2)
{
	//пока не сортирую
	return 0;
}