#include <stdlib.h>
#include <stdio.h>
#include "qmem.c"

//allocation
int qmem_alloc(unsigned num_bytes, void ** rslt);
int qmem_allocz(unsigned num_bytes, void ** rslt);
int qmem_allocv(unsigned num_bytes, int mark, void ** rslt);

//deallocation
int qmem_free(void ** data);

//utility
int qmem_cmp(void * p1, void * p2, int * diff);
int qmem_cpy(void * dst, void * src);
int qmem_scrub(void * data);
int qmem_scrubv(void * data, int mark);
int qmem_size(void * data, unsigned * rslt);
int qmem_stats(unsigned * num_allocs, unsigned * num_bytes_alloced);