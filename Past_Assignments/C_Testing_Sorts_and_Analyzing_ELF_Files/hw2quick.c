#include "hw2.h"
#include <stdio.h>
#include <stdlib.h>
#include <string.h>

int part(char ar[][256], int low, int high)
{
	char piv[256];
	strcpy(piv,ar[high]);
	int i = (low - 1);
	int j = low;
	char temp[256];
	for(j; j<= high - 1; j++)
	{
		if((int)ar[j][0] <= (int)piv[0])
		{
			i++;
			strcpy(temp,ar[i]);
			strcpy(ar[i],ar[j]);
			strcpy(ar[j],temp);
		}
	}
	strcpy(temp,ar[i+1]);
	strcpy(ar[i+1],ar[high]);
	strcpy(ar[high],temp);
	return (i+1);
}

void quickSort(char ar[][256], int low, int high)
{
	if(low < high)
	{
		int piv = part(ar,low,high);
		quickSort(ar,low,piv -1);
		quickSort(ar,piv + 1,high);
	}
}