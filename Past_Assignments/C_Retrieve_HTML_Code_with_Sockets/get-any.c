/*
* Gabriel Hasenoehrl
* CS 270
* Homework 5
* 11/30/2018
* 
* Uses sockets to open a connection with a website using HTTP 1.1, and reads the HTML code.
* This program also has basic sorting options, as well as a count of ascii characters, for the HTML code
*/

#include <stdio.h>
#include <stdlib.h>
#include <sys/types.h>
#include <sys/stat.h>
#include <string.h>
#include <sys/socket.h>
#include <netdb.h>
#include <arpa/inet.h>
#include <unistd.h>
#include <sys/time.h>

#define MAX_RESP_LEN (655360000)

struct information
{
	char asciiChar;
	int amount;
};

char msg_buf[MAX_RESP_LEN];

int
main(argc, argv)
    int argc;
    char * argv[];
{
	if(argc == 3)
	{
		//program ran without any - [ FIS ] options 
	}
	else if(argc != 4)
	{
		if(argc < 4)
		{
			printf("Too few arguments given.\n");
		}
		else if (argc > 4)
		{
			printf("Too many arguments given.\n");
		}
		printf("Arguments should be in the order:\n\nget-any hostname resource -[FIS]\n\nresource may just be a \"/\"\n\n");
		return -1;
	}
	
	struct information array[255];
	if(argc == 3)
	{
		//no flags specified
	}
	else if(strcmp (argv[3],"-F") == 0)
	{
		//declares struct array to be used to find the count of HTML characters later on 
		 int i = 0;
		 for(i; i < 255; i++)
		 {
			 array[i].asciiChar = (char)i;
			 array[i].amount = 0;
		 }
		 
	 }
	else if(strcmp (argv[3],"-I") == 0)
	 {
		 //correct flag
	 }
	 else if(strcmp (argv[3],"-S") == 0)
	 {
		 //correct flag
	 }
	 else
	 {
		 printf("Unknown Flag of \"%s\" given as argument\nValid options are:\n-F  :  To count the frequency of all bits\n-I  :  To invert the sequence of all bits\n-S  :  To sort the sequence of all bits\n",argv[3]);
		 return -1;
	 }
	 
    int newsocket;
    ssize_t length;
    struct timeval timeout;
    unsigned long total_length;
    struct addrinfo hints, *result;

    memset(&hints, 0, sizeof(hints));
    hints.ai_family = AF_INET;
    hints.ai_socktype = SOCK_STREAM;

    /*
     * resolve hostname to IP address: specify host and port num
     * (port num as string, not int)
     */
    if (getaddrinfo(argv[1], "80", &hints, &result) != 0)
    {
		//can get a seg fault here with freeaddrinfo()
        freeaddrinfo(result);
        perror("getaddrinfo: ");
        exit(-1);
    }

    /*
     * create socket
     */
    if ((newsocket = socket(result->ai_family, result->ai_socktype, 0)) == -1)
    {
        perror("socket: ");
        freeaddrinfo(result);
        close(newsocket);
        exit(-1);
    }

    /*
     * set socket timeouts
     */
    memset(&timeout, 0, sizeof(timeout));
    timeout.tv_sec = 60;
    timeout.tv_usec = 0;
    /* set timeout for a SEND */
    setsockopt(newsocket, SOL_SOCKET, SO_SNDTIMEO, &timeout, sizeof(timeout));
    /* set timeout for a RECV */
    setsockopt(newsocket, SOL_SOCKET, SO_RCVTIMEO, &timeout, sizeof(timeout));

    /*
     * connect to website
     */
    if (connect(newsocket, result->ai_addr, result->ai_addrlen) == -1)
    {
        perror("connect: ");
        freeaddrinfo(result); // free addrinfo memory
        close(newsocket);
        exit(-1);
    }

    /*
     * send an HTTP GET request
     */

	 char websiteInfo[300] = "GET ";
	 char *str2 = " HTTP/1.1\r\nHost: ";
	 char *str3 = "\r\nReferer: \r\nConnection: Close\r\n\r\n";
	 strcat(websiteInfo, argv[2]);	//argv[2] is the resource
	 strcat(websiteInfo,str2);
	 strcat(websiteInfo, argv[1]);	//argv[1] is the website
	 strcat(websiteInfo,str3);
	if ((send(newsocket, websiteInfo, 135, 0)) == -1) 
    {
        perror("send: ");
        freeaddrinfo(result);
        close(newsocket);
        exit(-1);
    }

    /*
     * read webpage from connection
     */
    total_length = 0UL;
    for (;;)
    {
        length = recv(newsocket, msg_buf, MAX_RESP_LEN-1, MSG_WAITALL);
		if (length <= 0)
        {    
			break;
		}
        msg_buf[length] = 0;
		fprintf(stdout, "%s", msg_buf);
        total_length += length;
		
		//checks if there are flags specified
		if(argc == 4)
		{
			//adds HTML characters into the struct array declared earlier by the -F flag
			if(strcmp (argv[3],"-F") == 0)
			{
				
				int i = 0;
				while(i < length)
				{
					int j = 0;
					while(j < 255)
					{
						if(msg_buf[i] == array[j].asciiChar)
						{
							array[j].amount = array[j].amount + 1;
							break;
						}
						j++;
					}
					i++;
				}	
			}
			
		}
    }
	printf("\n");
	if(argc == 3)
	{
		 printf("\n\n");
		 printf("final status: %ld.\n", length);
		 printf("read %ld chars.\n", total_length);
		 return 1;
	}

	 if(strcmp (argv[3],"-F") == 0)
	 {
		 printf("get-any: F:\n");
		 int i = 0;
		 int j = 0;
		 for(i; i < 255-1; i++)
		 {
			 for(j; j < 255-1; j++)
			 {
				if(array[ j ].amount < array[ j+1 ].amount)
			    {
					struct information temp = array[ j ];
					array[ j ] = array[ j+1 ];
					array[ j+1 ] = temp;
			   	}
		     }
			 j = 0;
	 	 }
		 i = 0;
		 while(i < 255)
		 {
			 if(array[i].amount == 0)
			 {
				//wont print characters not found
			 }
			 else
			 {
				printf("%c=%d\n",array[i].asciiChar,array[i].amount);
			 }
			i++;
		 }
	 }
	 else if(strcmp (argv[3],"-I") == 0)
	 {
		printf("get-any: I:\n");
		int length = total_length;
		while(length >= 0)
		{
			printf("%c",msg_buf[length]);
			length--;
		}
	 }
	 else if(strcmp (argv[3],"-S") == 0)
	 {
		 printf("get-any: S:\n");
		 int i = 0;
		 int j = 0;
		 for(i; i < total_length - 1; i++)
		 {
			 for(j; j < total_length - 1; j++)
			 {
				if(msg_buf[ j ] > msg_buf[ j+1 ])
			    {
					char temp = msg_buf[ j ];
					msg_buf[ j ] = msg_buf[ j+1 ];
					msg_buf[ j+1 ] = temp;
			   	}
		     }
			 j = 0;
	 	 }
		printf("%s",msg_buf);
		i++;
	}
    if (length == -1)
    {
        perror("recv: ");
        freeaddrinfo(result);
        close(newsocket);
        exit(-1);
    }

    /*
     * free addrinfo memory
     */
    freeaddrinfo(result);

    /*
     * close socket
     */
    close(newsocket);

    return 0;
}
