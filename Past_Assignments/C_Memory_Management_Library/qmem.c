/*	HW3 CS270
*	Gabriel Hasenoehrl
*	10/08/2018
*	
*	3 functions to allocate memory
*	1 function to deallocate memory
*	6 functions for utility
*
*	uses a link listed to keep track of the memory that has been allocated
*
*/

#include <stdio.h>
#include <stdlib.h>

struct LinkedList
{
	long id;		//stores address 
	int size; 	//in bytes
	struct LinkedList* next;
};

typedef struct LinkedList *node;
node head = NULL;

void addNode(long ID,int SIZE)
{
	node temp = malloc(sizeof(node));
	temp -> id = ID;
	temp -> size = SIZE;
	temp -> next = NULL;
	if(head == NULL)
	{
		head = temp;
	}
	else
	{
		//adds new nodes to the front of the list as they are
		//the most likely to be used again.  So cuts down on
		//searches.
		temp -> next = head;
		head = temp;
	}
}

int checkList(void **value)
{
	node temp = head;
	long checker = (long)*value;
	while(temp != NULL)
	{
		if(checker == temp -> id)
		{
			return 1;   //in the list
		}
		temp = temp -> next;
	}
	return 0;  //not in the list
}

int deleteNode(void **value)
{
	node prev = head;
	node temp = head;
	long checker = (long)*value;
	while(temp != NULL)
	{
		if(checker == temp -> id)
		{
			//found id and are looking for how to properly remove the node
			if(temp == head)
			{
				head = temp -> next;
				free(temp);
			}
			else if(temp -> next == NULL)
			{
				prev -> next = NULL;
				free(temp);
			}
			else
			{
				prev -> next = temp -> next;
				free(temp);
			}
			return 1;
			
		}
		prev = temp;
		temp = temp -> next;
	}
	return 0;
}

int getSize(void *value)
{
	node temp = head;
	long checker = (long)value;
	while(temp != NULL)
	{
		if(temp -> id == checker)
		{
			return temp -> size;
		}
		temp = temp -> next;
	}
	return -1;
}

void displayLinked()
{
	node temp = head;
	int i = 1;
	printf("No.      ADDRESS\t\t  SIZE\n");
	printf("=============================================\n");
	while(temp != NULL)
	{
		
		printf("%-7d  %-25ld%-20d\n",i,temp -> id,temp -> size);
		temp = temp -> next;
		i++;
	}
	printf("\n");
	return;
}

int qmem_alloc(unsigned num_bytes, void ** rslt)
{
	int listchecker = checkList(rslt);
	if(listchecker == 1)
	{
		//Deletes the old address of rslt if it has allready been alocated before.
		//The reason for this is to delete the previous address stored, to not
		//clutter the linked list with addresses no longer in use.
		deleteNode(rslt);	
	}
	void *p = malloc(num_bytes);
	if(p == NULL)
	{
		return -1;
	}
	if(p != NULL)
	{
		rslt = p;
		return 0;
	}
	else
	{
		return -2;
	}
}

int qmem_allocz(unsigned num_bytes, void ** rslt)
{
	int listchecker = checkList(rslt);
	if(listchecker == 1)
	{
		deleteNode(rslt);	
	}
	void *p = malloc(num_bytes);
	if(p == NULL)
	{
		return -1;
	}
	
	if(p != NULL)
	{
		int i = 0;
		while(i < num_bytes)
		{
			*((char*)p + i) = 0;
			i++;
		}
		rslt = p;
		long address = (long)&p;
		addNode(address , num_bytes);
		return 0;
	}
	
	else
	{
		return -2;
	}
}

int qmem_allocv(unsigned num_bytes, int mark, void ** rslt)
{
	int listchecker = checkList(rslt);
	if(listchecker == 1)
	{
		deleteNode(rslt);	
	}
	void *p = (void*)malloc(num_bytes);
	if(p == NULL)
	{
		return -1;
	}
	if(p != NULL)
	{
		int i = 0;
		while(i < num_bytes)
		{
			*((char*)p + i) = mark;
			i++;
		}
		*rslt = p;
		//printf("%d\n",*(char*)rslt);
		long address = (long)p;
		addNode(address, num_bytes);
		return 0;
	}
	else
	{
		return -2;
	}
}

int qmem_free(void** data)
{
	if(*data == NULL)
	{
		//address of data is NULL
		return -2;
	}
	else if(**(char**)data == 0)
	{
		//what data is pointing to is NULL
		return -1;
	}
	int listchecker = checkList(data);
	if(listchecker == 1)
	{
		deleteNode(data);
		**(char**)data = '\0';
		return 0;
	}
	else
	{
		//qmem has not been used on this data
		return -3;
	}
}
struct test
{
	int id;
	int stuff;
};

int qmem_cmp(void * p1, void * p2, int * diff)
{
	//error returns
	if(p1 == NULL)
	{
		return -1;
	}
	if(p2 == NULL)
	{
		return -2;
	}
	int listchecker = checkList((void**)&p1);
	if(listchecker != 1)
	{
		return -3;	//not been alocated using qmem
	}
	int listchecker2 = checkList((void**)&p2);
	if(listchecker2 != 1)
	{
		return -4;	//not been alocated using qmem
	}
	
	//gets the smallest size of the pointers passed in to cmp
	int size = getSize(p1);
	int size2 = getSize(p2);
	int cmpSize = 0;
	if(size <= size2)
	{
		cmpSize = size;
	}
	else if(size2 < size)
	{
		cmpSize = size2;
	}
	int i = 0;
	while(i < cmpSize)
	{
		if(*((char*)p1 + i) == *((char*)p2 + i))
		{
			i++;
		}
		else
		{
			*diff = i + 1;
			return 0;
		}
	}
	if(i == cmpSize)
	{
		*diff = 0;
		return 0;
	}
}

int qmem_cpy(void * dst, void * src)
{
	//error returns
	if(dst == NULL)
	{
		return -1;
	}
	if(src == NULL)
	{
		return -2;
	}
	int listchecker = checkList((void**)&dst);
	if(listchecker != 1)
	{
		return -3;	//not been alocated using qmem
	}
	int listchecker2 = checkList((void**)&src);
	if(listchecker2 != 1)
	{
		return -4;	//not been alocated using qmem
	}
	if(dst == src)
	{
		return -5; //pointing to same memory address
	}
	//checks if they are the same size
	int size = getSize(dst);
	int size2 = getSize(src);
	if(size != size2)
	{
		return -6;
	}
	int i = 0;
	while(i < size)
	{
		*((char*)dst + i) = *((char*)src + i);
		i++;
	}
}

int qmem_scrub(void * data)
{
	//error returns
	if(data == NULL)
	{
		return -1;
	}
	int listchecker = checkList((void**)&data);
	if(listchecker != 1)
	{
		return -2;	//not been alocated using qmem
	}

	//checks if they are the same size
	int size = getSize(data);
	int i = 0;
	while(i < size)
	{
		*((char*)data + i) = 0;
		i++;
	}
}

int qmem_scrubv(void * data, int mark)
{
	//error returns
	if(data == NULL)
	{
		return -1;
	}
	int listchecker = checkList((void**)&data);
	if(listchecker != 1)
	{
		return -2;	//not been alocated using qmem
	}

	//checks if they are the same size
	int size = getSize(data);
	int i = 0;
	while(i < size)
	{
		*((char*)data + i) = mark;
		i++;
	}
}

int qmem_size(void * data, unsigned * rslt)
{
	//error returns
	if(data == NULL)
	{
		return -1;
	}
	else if(*(char*)data == 0)
	{
		return -2;
	}
	int listchecker = checkList((void**)&data);
	if(listchecker != 1)
	{
		return -3;	//not been alocated using qmem
	}
	
	int size = getSize(data);
	*rslt = size;
	return 1;
}

int qmem_stats(unsigned * num_allocs, unsigned * num_bytes_alloced)
{
	if(num_allocs == NULL)
	{
		return -1;
	}
	if(num_bytes_alloced == NULL)
	{
		return -2;
	}
	node temp = head;
	int i = 0;
	int total = 0;
	if(temp == NULL)
	{
		*num_allocs = i;
		*num_bytes_alloced = total;
		return 0;
	}
	while(temp != NULL)
	{
		total = total + temp -> size;
		temp = temp -> next ;
		i++;
	}
	*num_allocs = i;
	*num_bytes_alloced = total;
	return 0;
}
