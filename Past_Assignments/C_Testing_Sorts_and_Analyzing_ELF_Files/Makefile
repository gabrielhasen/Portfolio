main: hw2.o hw2b.o hw2i.o hw2q.o
	date >> readme.txt
	gcc  hw2.h hw2bub.o hw2inser.o hw2quick.o hw2.o
	./a.out >> readme.txt

hw2.o: hw2.c hw2.h hw2bub.c hw2inser.c hw2quick.c
	gcc -c  hw2.h hw2.c hw2bub.c hw2inser.c hw2quick.c
	gcc  hw2.h hw2.o hw2bub.c hw2inser.c hw2quick.c -Wl,-M > linkmap.txt
	readelf -a hw2.o > readelf.txt
	
hw2b.o: hw2bub.c hw2.h
	gcc -c hw2bub.c
	readelf -a hw2bub.o > readelfBub.txt

hw2i.o: hw2inser.c hw2.h
	gcc -c hw2inser.c
	readelf -a hw2inser.o > readelfIns.txt
	
hw2q.o: hw2quick.c
	gcc -c hw2quick.c
	readelf -a hw2quick.o > readelfQuick.txt
	
clean:
	rm *.o readelfBub.txt readelfIns.txt readelfQuick.txt linkmap.txt a.out