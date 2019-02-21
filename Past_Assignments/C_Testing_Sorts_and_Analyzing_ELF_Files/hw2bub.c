#include "hw2.h"
#include <stdio.h>
#include <stdlib.h>
#include <string.h>

void bubbleSort(char ar[][256], int n)
{
	int check = 0;
	int j = 0, i = 0;
	char temp[256];
	for(i = 0; i < n -1; i++)
	{
		for(j = 0; j < n-i-1; j++)
		{
			if((int)ar[j][0] > (int)ar[j+1][0])
			{
					strcpy(temp,ar[j]);
					strcpy(ar[j],ar[j+1]);
					strcpy(ar[j+1],temp);
				
			}
		}
		check++;
		//printf("%d\n", check);
	}
}