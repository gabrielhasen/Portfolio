#include "hw2.h"
#include <stdio.h>
#include <stdlib.h>
#include <string.h>

void insertionSort(char ar[][256], int n)
{
	int i, j;
	char temp[256];
	int count = 0;
	for(i = 1; i < n; i++)
	{
		strcpy(temp,ar[i]);
		j = i-1;
		while(j  >= 0 && (int)ar[j][0] > (int)temp[0])
		{
			strcpy(ar[j+1],ar[j]);
			j = j -1;
		}
		//printf("%d\n",count);
		count++;
		strcpy(ar[j+1],temp);
	}
}