/*
Gabriel Hasenoehrl
CS 210
October 1, 2018


Program Description
===================
This program reads a ccx/cci file and produces the tokens
for that program.  In order to accomplish this the file is
written into an array.  Over the course of seven passes
over that array, information is stripped and replaced with 
a ' ' in order to make the next pass easier and maintain 
the correct index position.  During this process of stripping 
the information from the array, when a token is found it is placed 
into a link list based off of the index the starting character 
for the token was found in the array.

Reflections
===================
Maintaining the correct index position on the main array was
difficult at times.  Since this needed to be done in order for
my linked list logic to work.

The code was a bit longer than I feel it could have been if
I approached this assignment differently.  After completing 
this assignment I feel like it would have been more neat and
concise to break information into tokens while reading it in
instead of having multiple passes over the whole file.

Having multiple passes over a larger file as well would be 
far slower than breaking the program into tokens while reading
the file initially.
*/

#include <stdio.h>
#include <stdlib.h>

//Linked list to store the sorted tokens
struct LinkedList
{
	//The large input size is to handle comments as it is not dynamically allocated
	char input[2560];
	char type[20];
	int index;
	struct LinkedList *next;
};

typedef struct LinkedList *node;
node head;
node tail;

node createNode(char value[], char ty[], int ind)
{
	node temp;
	temp = (node)malloc(sizeof(struct LinkedList));
	int i = 0;
	int j = 0;
	
	while(i < 256)
	{
		temp -> input[i] = value[i];
		if(j < 20)
		{
			temp -> type[j] = ty[j];
			j++;
		}
		i++;
	}
	
	temp -> index = ind;
	temp -> next = NULL;
	return temp;
}

//addNode adds a node to a link list based off the index of the array of the whole txt file.
//This way over the multiple passes my program does to break everything down into tokens,
//the tokens stay in order.
node addNode(char value[], char ty[], int ind)
{
	node temp, ptr, prev;
	temp = createNode(value,ty,ind);
	
	if(head == NULL)
	{
		head = temp;
		tail = temp;
	}
	else
	{
		ptr = head;
		prev = ptr;
		while(ptr -> next != NULL)
		{
			if(prev -> index < temp -> index && temp -> index < ptr -> index)
			{
				prev -> next = temp;
				temp -> next = ptr;
				return;
			}
			prev = ptr;
			ptr = ptr -> next;
		}
		
		if(ptr -> index > temp -> index)
		{
			prev -> next = temp;
			temp -> next = ptr;
			ptr -> next = NULL;
			return;
		}
		
		ptr -> next = temp;
		tail = temp;
	}
	return head;
}

void displayLinked()
{
	node p;
	p = head -> next;
	
	while(p != NULL)
	{
		printf("%s %s\n", p -> input, p -> type);
		p = p -> next;
	}
}

void commentCheck(char document[])
{
	//swaps and checks two characters in an array for mult-character comments
	char check[2];
	check[0] = document[0];
	check[1] = document[1];
	
	int i = 2;
	while(document[i] != '\0')
	{
		if(check[0] == '/' && check[1] == '*')
		{
			//printf("found comment");
			char save[256] = {'\0'};
			int saveind = i-2;
			save[0] = check[0];
			save[1] = check[1];
			document[i-1] = ' ';
			document[i-2] = ' ';
			int checker = 0;
			int j = 2;
			while(checker == 0)
			{
				if(check[0] == '*' && check[1] == '/')
				{
					//printf("found end");
					addNode(save,"(comment)",saveind);
					checker = 1;
					break;
				}
				save[j] = document[i];
				check[0] = check[1];
				check[1] = document[i];
				document[i] = ' ';
				j++;
				i++;
			}
		}
		check[0] = check[1];
		check[1] = document[i];
		i++;
	}
}

void stringCheck(char document[])
{
	//detects tokens based of of the " character and the next "
	int i = 0;
	while (document[i] != '\0')
	{
		if(document[i]=='"')
		{
			char save[256] = {'\0'};
			int j = 0;
			int saveind = i;
			save[j] = document[i];
			document[i] = ' ';
			i++;
			while(document[i] != '"')
			{
				j++;
				save[j] = document[i];
				document[i] = ' ';
				i++;
			}
			j++;
			save[j] = document[i];
			document[i] = ' ';
			addNode(save,"(string)",saveind);
		}
		i++;
	}
}

void charCheck(char document[])
{
	//detects tokens based of of the ' character and the next '
	int i = 0;
	while (document[i] != '\0')
	{
		if(document[i]=='\'')
		{
			char save[256] = {'\0'};
			int j = 0;
			int saveind = i;
			save[j] = document[i];
			document[i] = ' ';
			i++;
			while(document[i] != '\'')
			{
				j++;
				save[j] = document[i];
				document[i] = ' ';
				i++;
			}
			j++;
			save[j] = document[i];
			document[i] = ' ';
			addNode(save,"(character literal)",saveind);
		}
		i++;
	}
}

void keywordCheck(char document[])
{
	//adds character to an array until a ' ','\t','\n' is found.
	//after it then checks if that word is a keyword.
	int i = 0;
	int j = 0;
	int saveind = 0;
	char keyword[256] = {'\0'};
	while(document[i] != '\0')
	{
		if(document[i] == ' ' || document[i] == '\t' || document[i] == '\n')
		{
			int check = 0;
			j = 0;
			//printf("%s\n",keyword);
			
			if(strcmp(keyword,"accessor") == 0)
			{
				check = 1;
				addNode(keyword,"(keyword)",saveind);
			}
			else if(strcmp(keyword,"and") == 0)
			{
				check = 1;
				addNode(keyword,"(keyword)",saveind);
			}
			else if(strcmp(keyword,"array") == 0)
			{
				check = 1;
				addNode(keyword,"(keyword)",saveind);
			}
			else if(strcmp(keyword,"begin") == 0)
			{
				check = 1;
				addNode(keyword,"(keyword)",saveind);
			}
			else if(strcmp(keyword,"bool") == 0)
			{
				check = 1;
				addNode(keyword,"(keyword)",saveind);
			}
			else if(strcmp(keyword,"case") == 0)
			{
				check = 1;
				addNode(keyword,"(keyword)",saveind);
			}
			else if(strcmp(keyword,"character") == 0)
			{
				check = 1;
				addNode(keyword,"(keyword)",saveind);
			}
			else if(strcmp(keyword,"constant") == 0)
			{
				check = 1;
				addNode(keyword,"(keyword)",saveind);
			}
			else if(strcmp(keyword,"else") == 0)
			{
				check = 1;
				addNode(keyword,"(keyword)",saveind);
			}
			else if(strcmp(keyword,"elsif") == 0)
			{
				check = 1;
				addNode(keyword,"(keyword)",saveind);
			}
			else if(strcmp(keyword,"end") == 0)
			{
				check = 1;
				addNode(keyword,"(keyword)",saveind);
			}
			else if(strcmp(keyword,"exit") == 0)
			{
				check = 1;
				addNode(keyword,"(keyword)",saveind);
			}
			else if(strcmp(keyword,"function") == 0)
			{
				check = 1;
				addNode(keyword,"(keyword)",saveind);
			}
			else if(strcmp(keyword,"if") == 0)
			{
				check = 1;
				addNode(keyword,"(keyword)",saveind);
			}
			else if(strcmp(keyword,"in") == 0)
			{
				check = 1;
				addNode(keyword,"(keyword)",saveind);
			}
			else if(strcmp(keyword,"integer") == 0)
			{
				check = 1;
				addNode(keyword,"(keyword)",saveind);
			}
			else if(strcmp(keyword,"interface") == 0)
			{
				check = 1;
				addNode(keyword,"(keyword)",saveind);
			}
			else if(strcmp(keyword,"is") == 0)
			{
				check = 1;
				addNode(keyword,"(keyword)",saveind);
			}
			else if(strcmp(keyword,"loop") == 0)
			{
				check = 1;
				addNode(keyword,"(keyword)",saveind);
			}
			else if(strcmp(keyword,"module") == 0)
			{
				check = 1;
				addNode(keyword,"(keyword)",saveind);
			}
			else if(strcmp(keyword,"mutator") == 0)
			{
				check = 1;
				addNode(keyword,"(keyword)",saveind);
			}
			else if(strcmp(keyword,"natural") == 0)
			{
				check = 1;
				addNode(keyword,"(keyword)",saveind);
			}
			else if(strcmp(keyword,"null") == 0)
			{
				check = 1;
				addNode(keyword,"(keyword)",saveind);
			}
			else if(strcmp(keyword,"of") == 0)
			{
				check = 1;
				addNode(keyword,"(keyword)",saveind);
			}
			else if(strcmp(keyword,"or") == 0)
			{
				check = 1;
				addNode(keyword,"(keyword)",saveind);
			}
			else if(strcmp(keyword,"others") == 0)
			{
				check = 1;
				addNode(keyword,"(keyword)",saveind);
			}
			else if(strcmp(keyword,"out") == 0)
			{
				check = 1;
				addNode(keyword,"(keyword)",saveind);
			}
			else if(strcmp(keyword,"positive") == 0)
			{
				check = 1;
				addNode(keyword,"(keyword)",saveind);
			}
			else if(strcmp(keyword,"procedure") == 0)
			{
				check = 1;
				addNode(keyword,"(keyword)",saveind);
			}
			else if(strcmp(keyword,"range") == 0)
			{
				check = 1;
				addNode(keyword,"(keyword)",saveind);
			}
			else if(strcmp(keyword,"return") == 0)
			{
				check = 1;
				addNode(keyword,"(keyword)",saveind);
			}
			else if(strcmp(keyword,"struct") == 0)
			{
				check = 1;
				addNode(keyword,"(keyword)",saveind);
			}
			else if(strcmp(keyword,"subtype") == 0)
			{
				check = 1;
				addNode(keyword,"(keyword)",saveind);
			}
			else if(strcmp(keyword,"then") == 0)
			{
				check = 1;
				addNode(keyword,"(keyword)",saveind);
			}
			else if(strcmp(keyword,"type") == 0)
			{
				check = 1;
				addNode(keyword,"(keyword)",saveind);
			}
			else if(strcmp(keyword,"when") == 0)
			{
				check = 1;
				addNode(keyword,"(keyword)",saveind);
			}
			else if(strcmp(keyword,"while") == 0)
			{
				check = 1;
				addNode(keyword,"(keyword)",saveind);
			}
			
			//this is to replace the main array with
			//spaces to make other sorts easier
			if(check == 1)
			{
				check = 0;
				int x = i;
				while(x + 1 != saveind)
				{
					document[x] = ' ';
					x--;
				}
			}
			
			int reset;
			for(reset = 0; reset < 256; reset++)
			{
				keyword[reset] = '\0';
			}
			i++;
			saveind = i;
		}
		if((int)document[i] > 32)
		{
			keyword[j] = document[i];
			j++;
		}
		i++;
	}
}

void operatorCheck(char document[])
{
	//most complex function in the program.
	//
	//check[0] is trailing behind the current position check[1].
	//
	//Will check single operators in check[0] after checking
	//all of the potential double operators.
	//
	//Reason for complex save ind and i manipulation is for
	//assigning the correct start index to the linked list.
	//
	//Using a link list for organization and sorting by the
	//array index made these checks difficult.  Would take
	//a different approach next time to implement this
	//lexer because of these issues.
	char check[2];
	check[0] = document[0];
	check[1] = document[1];
	int i = 2;
	int saveind = 0;
	while(check[0] != '\0')
	{
		char save[2] = {'\0'};
		if(check[0] == '=' && check[1] == '>')
		{
			document[i-1] = ' ';
			document[i-2] = ' ';
			save[0] = check[0];
			save[1] = check[1];
			saveind = i-2;
			i = i + 1;
			check[1] = document[i];
			check[0] = check[1];
			addNode(save,"(operator)",saveind);
			i++;
		}
		else if(check[0] == '!' && check[1] == '=')
		{
			document[i-1] = ' ';
			document[i-2] = ' ';
			save[0] = check[0];
			save[1] = check[1];
			saveind = i-2;
			i = i + 1;
			check[1] = document[i];
			check[0] = check[1];
			addNode(save,"(operator)",saveind);
			i++;
		}
		else if(check[0] == '*' && check[1] == '*')
		{
			document[i-1] = ' ';
			document[i-2] = ' ';
			save[0] = check[0];
			save[1] = check[1];
			saveind = i-2;
			i = i + 1;
			check[1] = document[i];
			check[0] = check[1];
			addNode(save,"(operator)",saveind);
			i++;
		}
		else if(check[0] == '>' && check[1] == '=')
		{
			document[i-1] = ' ';
			document[i-2] = ' ';
			save[0] = check[0];
			save[1] = check[1];
			saveind = i-2;
			i = i + 1;
			check[1] = document[i];
			check[0] = check[1];
			addNode(save,"(operator)",saveind);
			i++;
		}
		else if(check[0] == '<' && check[1] == '=')
		{
			document[i-1] = ' ';
			document[i-2] = ' ';
			save[0] = check[0];
			save[1] = check[1];
			saveind = i-2;
			i = i + 1;
			check[1] = document[i];
			check[0] = check[1];
			addNode(save,"(operator)",saveind);
			i++;
		}
		else if(check[0] == '<' && check[1] == '>')
		{
			document[i-1] = ' ';
			document[i-2] = ' ';
			save[0] = check[0];
			save[1] = check[1];
			saveind = i-2;
			i = i + 1;
			check[1] = document[i];
			check[0] = check[1];
			addNode(save,"(operator)",saveind);
			i++;
		}
		else if(check[0] == '>' && check[1] == '>')
		{
			document[i-1] = ' ';
			document[i-2] = ' ';
			save[0] = check[0];
			save[1] = check[1];
			saveind = i-2;
			i = i + 1;
			check[1] = document[i];
			check[0] = check[1];
			addNode(save,"(operator)",saveind);
			i++;
		}
		else if(check[0] == '<' && check[1] == '<')
		{
			document[i-1] = ' ';
			document[i-2] = ' ';
			save[0] = check[0];
			save[1] = check[1];
			saveind = i-2;
			i = i + 1;
			check[1] = document[i];
			check[0] = check[1];
			addNode(save,"(operator)",saveind);
			i++;
		}
		else if(check[0] == '.' && check[1] == '.')
		{
			document[i-1] = ' ';
			document[i-2] = ' ';
			save[0] = check[0];
			save[1] = check[1];
			saveind = i-2;
			i = i + 1;
			check[1] = document[i];
			check[0] = check[1];
			addNode(save,"(operator)",saveind);
			i++;
		}
		else if(check[0] == ':' && check[1] == '=')
		{
			document[i-1] = ' ';
			document[i-2] = ' ';
			save[0] = check[0];
			save[1] = check[1];
			saveind = i-2;
			i = i + 1;
			check[1] = document[i];
			check[0] = check[1];
			addNode(save,"(operator)",saveind);
			i++;
		}
		else if(check[0] == '=')
		{
			document[i-2] = ' ';
			save[0] = check[0];
			saveind = i -2;
			addNode(save,"(operator)",saveind);
		}
		else if(check[0] == ']')
		{
			document[i-2] = ' ';
			save[0] = check[0];
			saveind = i -2;
			addNode(save,"(operator)",saveind);
		}
		else if(check[0] == '[')
		{
			document[i-2] = ' ';
			save[0] = check[0];
			saveind = i -2;
			addNode(save,"(operator)",saveind);
		}
		else if(check[0] == ':')
		{
			document[i-2] = ' ';
			save[0] = check[0];
			saveind = i -2;
			addNode(save,"(operator)",saveind);
		}
		else if(check[0] == ',')
		{
			document[i-2] = ' ';
			save[0] = check[0];
			saveind = i -2;
			addNode(save,"(operator)",saveind);
		}
		else if(check[0] == ';')
		{
			document[i-2] = ' ';
			save[0] = check[0];
			saveind = i - 2;
			addNode(save,"(operator)",saveind);
		}
		else if(check[0] == '&')
		{
			document[i-2] = ' ';
			save[0] = check[0];
			saveind = i -2;
			addNode(save,"(operator)",saveind);
		}
		else if(check[0] == '|')
		{
			document[i-2] = ' ';
			save[0] = check[0];
			saveind = i -2;
			addNode(save,"(operator)",saveind);
		}
		else if(check[0] == '/')
		{
			document[i-2] = ' ';
			save[0] = check[0];
			saveind = i -2;
			addNode(save,"(operator)",saveind);
		}
		else if(check[0] == '*')
		{
			document[i-2] = ' ';
			save[0] = check[0];
			saveind = i -2;
			addNode(save,"(operator)",saveind);
		}
		else if(check[0] == '-')
		{
			document[i-2] = ' ';
			save[0] = check[0];
			saveind = i -2;
			addNode(save,"(operator)",saveind);
		}
		else if(check[0] == '+')
		{
			document[i-2] = ' ';
			document[i-2] = ' ';
			save[0] = check[0];
			saveind = i -2;
			addNode(save,"(operator)",saveind);
		}
		else if(check[0] == ')')
		{
			document[i-2] = ' ';
			save[0] = check[0];
			saveind = i - 2;
			addNode(save,"(operator)",saveind);
		}
		else if(check[0] == '(')
		{
			document[i-2] = ' ';
			save[0] = check[0];
			saveind = i -2;
			addNode(save,"(operator)",saveind);
		}
		else if(check[0] == '>')
		{
			document[i-2] = ' ';
			save[0] = check[0];
			saveind = i -2;
			addNode(save,"(operator)",saveind);
		}
		else if(check[0] == '<')
		{
			document[i-2] = ' ';
			save[0] = check[0];
			saveind = i -2;
			addNode(save,"(operator)",saveind);
		}
		else if(check[0] == '.')
		{
			document[i-2] = ' ';
			save[0] = check[0];
			saveind = i - 2;
			addNode(save,"(operator)",saveind);
		}
		check[0] = check[1];
		check[1] = document[i];
		i++;
	}
}

void numericCheck(char document[])
{	
	//This logic could definitely be improved, however, it made sense to me
	//while writing the code to do it this way.  The biggest issue was trying
	//to have numbers in variable names, and not having some symbols be seen
	//as operators that are in numeric literals
	int i = 0;
	while (document[i] != '\0')
	{
		int check = 0;
		//ascii 35 = #
		if((int)document[i] == 35)
		{
			//ascii 48 through 57 are digits	ascii 35 = #	ascii 46 = .
			if((int)document[i+1] >= 48 && (int)document[i+1] <=57 || (int)document[i+1] == 35 || (int)document[i+1] == 46)
			{
				check = 1;
			}
			else
			{
				i++;
			}
		}
		//checks if previous was a uppercase or lowercase letter
		if( ((int)document[i-1] >= 65 && (int)document[i-1] <= 90) || ((int)document[i-1] >= 97 && (int)document[i-1] <= 122))
		{
			//checks for variable names with numbers
		}
		else
		{
			//checks have found that the digit should be saved.
			if((int)document[i] >= 48 && (int)document[i] <=57 || (int)document[i] == 35 || check == 1)
			{
				char save[256] = {'\0'};
				int j = 0;
				int saveind = i;
				int check = 0;
				while((int)document[i] >= 48 && (int)document[i] <=57 || (int)document[i] == 46 || (int)document[i] == 35)
				{
					if((int)document[i] == 46 || (int)document[i] == 35)
					{
						if((int)document[i+1] >= 48 && (int)document[i+1] <=57)
						{
							//is okay
						}
						else if((int)document[i] == 46 || (int)document[i] == 35)
						{
							//is okay
						}
						else
						{
							//is not okay
							check = 0;
							break;
						}
					}
					save[j] = document[i];
					document[i] = ' ';
					i++;
					j++;
				}
				//checks the save array before saving it to the linked list
				if((int)save[0] >= 48 && (int)save[0] <=57 || save[0] == 46 || save[0] == 35)
				{
					if(save[0] == 46 || save[0] == 35)
					{
						if(save[1] == '\0')
						{
							
						}
						else
						{
							addNode(save,"(numeric literal)",saveind);
						}
					}
					else
					{
						addNode(save,"(numeric literal)",saveind);
					}
				}
			}
		}
		i++;
	}
}

void identifierCheck(char document[])
{
	//this is the last check for the program to do.  All identifiers
	//will be the only things left in the program, due to the previous
	//functions replacing array characters they took out with ' '
	int i = 0;
	while (document[i] != '\0')
	{
		if((int)document[i] > 47)
		{
			char save[256] = {'\0'};
			int j = 0;
			int saveind = i;
			while(document[i] != ' ' && document[i] != '\0')
			{
				save[j] = document[i];
				document[i] = ' ';
				i++;
				j++;
			}
			if((int)save[0] <= 32)
			{
				
			}
			else
			{
				j++;
				save[j] = document[i];
				document[i] = ' ';
				addNode(save,"(identifier)",saveind);
			}
		}
		i++;
	}
}

int main(int argc, char *argv[])
{
	head = createNode(" "," ",-1);
	int num;
	FILE *fptr;
	fptr = fopen(argv[1], "r");
	char ch;
	int i = 0;
	char doc[3000000];
	
	while(!feof(fptr))
	{
		
		ch = getc(fptr);
		if(ch == EOF)
		{
			break;
		} 
		doc[i] = ch;
		i++;
	}
	fclose(fptr);
	int check = 0;
	i = 0;
	
	//checks
	commentCheck(doc);
	stringCheck(doc);
	charCheck(doc);
	numericCheck(doc);
	operatorCheck(doc);
	keywordCheck(doc);
	identifierCheck(doc);
	
	//sorted correct token output
	displayLinked();
}