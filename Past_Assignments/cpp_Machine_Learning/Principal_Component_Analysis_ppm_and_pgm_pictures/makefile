# what you want to name your tar/zip file:
TARNAME=mat31C
CXX=g++

BIN  = pca

SHELL=/bin/sh

# optimization flags
CXXFLAGS=-O3 -Wall
# debugging flags
CXXFLAGS=-g -Wall
LIBS=

SRCS=\
pca.cpp\
mat.cpp\
randf.cpp

HDRS=\
mat.h\
rand.h

OBJS=\
mat.o\
rand.o

$(BIN): $(OBJS) $(BIN).o
	$(CXX)  $(CFLAGS) $(OBJS) $(BIN).o $(LIBS) -o $(BIN)


clean:
	/bin/rm -f *.o a.out
