/*
Gabriel Hasenoehrl
10/28/2018
CS270 - System Software
HW 4 PTOR

*/

#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <unistd.h>
#include <sys/wait.h>
#include <sys/types.h>
#include <signal.h>

char result[5000] = {'\0'};
pid_t pid;
FILE *parent, *child;

//
//Logic Functions
//

int readLine(char **info)
{
	parent  = fopen("ptor-parent-NN","w");
	char str[5000] = {'\0'};
	printf("cmd: ");
	
	//reads line
	int i = 0;
	int j = 0;
	char c;
	while((c = getchar()) != '\0')
	{
		//printf("READLINE:\t%c\n",c);
		str[i] = c; 
		i++;
		//This is to break up by words
		if(c == ' ' || c == '\n')
		{
			str[i-1] = '\0';
			//copies to **info parameter
			info[j] = strdup(str);
			fputs(info[j],parent);
			fputs(" ",parent);
			j++;
			while(str[0] != '\0')
			{
				str[i] = '\0';
				i--;
			}
			
			//finds the end
			if(c == '\n')
			{
				fclose(parent);
				break;
			}
			i++;
		}
	}
	//if the user typed exit
	if(strcmp(info[0], "exit") == 0)
	{
		remove("Commands.txt");
		remove("ptor-parent-NN");
		remove("ptor-child-NN");
		exit(0);
	}
	return 0;
}

int nextBits(int x)
{
	unsigned int number = x;
	int i = number + 1;
	int counter2 = 0;
	unsigned int nextnumber = number + 1;
	int count = 0;
	while (number != 0)
	{
		if ((number & 1) == 1)
		{
			count++;
		}
		number = number >> 1;
	}
	while(counter2 != count)
	{
		counter2 = 0;
		while (nextnumber != 0)
		{
			if ((nextnumber & 1) == 1)
			{
				counter2++;
			}
			nextnumber = nextnumber >> 1;
		}
		i++;
		nextnumber = number + i;
	}
	char res = nextnumber - 1;
	sprintf(result, "%d", res);
	child = fopen("ptor-child-NN","w");
	fputs(result,child);
	FILE *com  = fopen("Commands.txt","a");
	fprintf(com,"1");
	fclose(com);
	memset(result, '\0', sizeof(result));
	fclose(child);
}

int addBits(int x, int y)
{
    long value = x;
	long value2 = y;
    int i = 0;
	int remainder = 0; 
	int total[100];
	while ( value != 0 ||  value2 != 0)
    {
        total[i] = ( value % 10 + value2 % 10 + remainder) % 2;
        remainder = ( value % 10 + value2 % 10 + remainder) / 2;
        value =  value / 10;
        value2 =  value2 / 10;
		i++;
    }
    if (remainder != 0)
	{
        total[i++] = remainder;
	}
    i--;
	child = fopen("ptor-child-NN","w");
	FILE *com  = fopen("Commands.txt","a");
    while(i >= 0)
	{
		fprintf(child,"%d",total[i]);
		i--;
	}
	fprintf(com,"1");
	fclose(com);
	fclose(child);
}

int hamming(char word[],char word2[])
{
	char arr[10000][256] = {'\0'};
	FILE *fptr, *fptr2;
	fptr = fopen(word, "r");
	fptr2 = fopen(word2, "r");
	child = fopen("ptor-child-NN","w");
	FILE *com  = fopen("Commands.txt","a");
	if(fptr == NULL)
	{
		fprintf(child,"File 1 not found");
		fprintf(com,"0");
		fclose(com);
		fclose(child);
		return;
	}
	else if(fptr2 == NULL)
	{
		fprintf(child,"File 2 not found");
		fprintf(com,"0");
		fclose(com);
		fclose(child);
		return;
	}
	char ch = '\0';
	int i = 0;
	int j = 0;

	int max = 0;
	int cur = 0;
	while(ch != EOF)
	{
		ch = getc(fptr);
		i++;
	}
	ch = '\0';
	while(ch != EOF)
	{
		ch = getc(fptr2);
		j++;
	}
	if(j == i)
	{
		fptr = fopen(word, "r");
		fptr2 = fopen(word2, "r");
		char ch2 = '\0';
		int counter = 0;
		int max = 0;
		int cur = 0;
		while(counter != i)
		{
			ch = getc(fptr);
			ch2 = getc(fptr2);
			if(ch == ch2 && ch != EOF )
			{
				cur++;
			}
			else
			{
				if(cur > max)
				{
					max = cur;
				}
				cur = 0;
			}
			counter++;
		}
		fclose(fptr);
		fclose(fptr2);
		fprintf(child,"Hamming distance: %d",max);
		fprintf(com,"1");
		fclose(com);
		fclose(child);
	}
	else
	{
		child = fopen("ptor-child-NN","w");
		FILE *com  = fopen("Commands.txt","a");
		fprintf(child,"file sizes differ");
		fprintf(com,"0");
		fclose(com);
		fclose(child);
	}
}

int runLength(char word[])
{
	FILE *fptr = fopen(word, "r");
	child = fopen("ptor-child-NN","w");
	FILE *com  = fopen("Commands.txt","a");
	if(fptr == NULL)
	{
		fprintf(child,"File not found");
		fprintf(com,"0");
		fclose(com);
		fclose(child);
		return;
	}
	char ch = '\0';
	int i = 0;
	while(ch != EOF)
	{
		ch = getc(fptr);
		i++;
	}
	ch = '\0';
	fptr = fopen(word, "r");
	int position = 0;
	int positionp = 0;
	int saveposition = 0;
	
	char byte;
	char bytep;
	char savebyte;
	fptr = fopen(word, "r");
	char ch2 = getc(fptr);
	int counter = 1;
	int max = 0;
	int cur = 0;
	int change = 0;
	while(counter != i)
	{
		position++;
		ch = ch2;
		ch2 = getc(fptr);
		if(ch2 != EOF)
		{
			byte = ch2;
		}
		if(ch == ch2)
		{
			if(change == 0)
			{
				bytep = byte;
				positionp = position;
				change = 1;
			}
			cur++;
		}
		else
		{
			if(cur > max)
			{
				saveposition = positionp;
				savebyte = bytep;
				max = cur;
			}
			change = 0;
			cur = 0;
		}
		counter++;
	}
	if(max != 0)
	{
		max++;
	}
	fprintf(child,"%d 0x%02x %d",saveposition,savebyte,max);
	fprintf(com,"1");
	fclose(com);
	fclose(child);
}

void display()
{
	FILE *com  = fopen("Commands.txt","r");
	child = fopen("ptor-child-NN","w");
	char ch = '\0';
	int fail = 0;
	int pass = 0;
	fprintf(child,"\n");
	while( (ch = getc(com)) != EOF)
	{
		if(ch == '1')
		{
			pass++;
		}
		else if(ch == '0')
		{
			fail++;
		}
	}
	fprintf(child,"Succeed: %d  Fail: %d",pass,fail);
	fclose(com);
	fclose(child);
}

void checksum(char word[])
{
	FILE *com  = fopen("Commands.txt","a");
	child = fopen("ptor-child-NN","w");
	FILE *fptr = fopen(word, "r");
	if(fptr == NULL)
	{
		fprintf(child,"File not found");
		fprintf(com,"0");
		fclose(com);
		fclose(child);
		return;
	}
	char ch = '\0';
	unsigned int sum = 0;
	while( (ch = getc(fptr)) != EOF)
	{
		sum = sum + (int)ch;
	}
	unsigned int x = (~sum)+1;
	fprintf(child,"0x%02x",x);
	fprintf(com,"1");
	fclose(com);
	fclose(child);
	fclose(fptr);
}

void command(char** parsed)
{
	FILE *com  = fopen("Commands.txt","a");
	child = fopen("ptor-child-NN","w");
	printf("ptor: %s: ",parsed[0]);
	fflush(stdout);
	if(parsed[0] == NULL)
	{
		printf("Error\n");
		fprintf(com,"0");
		return;
	}
	char first = *parsed[0];
	if(first == 'H')
	{
		hamming(parsed[1],parsed[2]);
	}
	else if(first == '0')
	{
		int value = atoi(parsed[1]);
		nextBits(value);
	}
	else if(first == 'R')
	{
		runLength(parsed[1]);
	}
	else if(first == 'X')
	{
		checksum(parsed[1]);
	}
	else if(first == '+')
	{
		int value = atoi(parsed[1]);
		int value2 = atoi(parsed[2]);
		addBits(value,value2);
	}
	else if(first == '!')
	{
		fprintf(com,"1");
		display();
	}
	else
	{
		fprintf(com,"0");
		fprintf(child," ");
		printf("Invalid Command\n");
	}
	fclose(com);

}

//
//Signal Handlers
//

void sigparentcheck();

void  sigchld_handler()
{
	//child opens file, reads command, executes function, and puts results into file.
	FILE *read = fopen("ptor-parent-NN","r");
	char ch;
	int i = 0;
	int j = 0;
	char *store[5000] = {'\0'};
	char str[5000] = {'\0'};
	while(1)
	{
		ch = getc(read);
		if(ch == ' ')
		{
			//new word
			store[j] = strdup(str);
			while(str[0] != '\0')
			{
				str[i] = '\0';
				i--;
			}
			i++;
			j++;
		}
		else if(ch == EOF)
		{
			break;
		}
		else
		{
			str[i] = ch;
			i++;
		}
	}
	command(store);
	sleep(0.5);
	signal(SIGUSR1,  sigchld_handler);
	kill(getppid(),SIGUSR2);
}

void sigp()
{
	//reads results from child file, and looks for signal to restart process again.
	char ch = '\0';
	FILE *child_R;
	child_R = fopen("ptor-child-NN","r");
	while(ch != EOF)
	{
		printf("%c",ch);
		ch = getc(child_R);
	}
	fclose(child_R);
	printf("\n");
	signal(SIGUSR1, sigparentcheck);
	kill(getpid(),SIGUSR1);
}

void sigparentcheck()
{
	//parent reads command puts it in file, sends signal to child and then waits for child signal.
	int status;
	int i = 0;
	char *info[5000] = {'\0'};
	readLine(info);
	signal(SIGUSR2, sigp);
	kill(pid,SIGUSR1);
	pause();
}

int main()
{
	int x = 0;
	signal(SIGUSR1, sigparentcheck);
	pid = fork();
	if(pid == -1)
	{
		printf("ERROR FORK()");
		return 0;
	}
	else if(pid == 0)
	{
		signal(SIGUSR1, sigchld_handler);
		kill(getppid(),SIGUSR1);
		pause();
	}
	else
	{
		pause();
	}
	while(1)
	{
		pause();
	}
}
