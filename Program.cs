//*****************************************************************************
//** 3362. Zero Array Transformation III                            leetcode **
//*****************************************************************************

#define MAXN 100005

typedef struct {
    int data[MAXN];
    int size;
} MaxHeap;

void maxHeapPush(MaxHeap* h, int val)
{
    int i = h->size++;
    while (i > 0)
    {
        int p = (i - 1) / 2;
        if (h->data[p] >= val) break;
        h->data[i] = h->data[p];
        i = p;
    }
    h->data[i] = val;
}

int maxHeapTop(MaxHeap* h)
{
    return h->size ? h->data[0] : -1;
}

void maxHeapPop(MaxHeap* h)
{
    int val = h->data[--h->size];
    int i = 0;
    while (i * 2 + 1 < h->size)
    {
        int a = i * 2 + 1, b = i * 2 + 2;
        int maxChild = (b < h->size && h->data[b] > h->data[a]) ? b : a;
        if (h->data[maxChild] <= val) break;
        h->data[i] = h->data[maxChild];
        i = maxChild;
    }
    h->data[i] = val;
}

typedef struct {
    int data[MAXN];
    int size;
} MinHeap;

void minHeapPush(MinHeap* h, int val)
{
    int i = h->size++;
    while (i > 0)
    {
        int p = (i - 1) / 2;
        if (h->data[p] <= val) break;
        h->data[i] = h->data[p];
        i = p;
    }
    h->data[i] = val;
}

int minHeapTop(MinHeap* h)
{
    return h->size ? h->data[0] : -1;
}

void minHeapPop(MinHeap* h)
{
    int val = h->data[--h->size];
    int i = 0;
    while (i * 2 + 1 < h->size)
    {
        int a = i * 2 + 1, b = i * 2 + 2;
        int minChild = (b < h->size && h->data[b] < h->data[a]) ? b : a;
        if (h->data[minChild] >= val) break;
        h->data[i] = h->data[minChild];
        i = minChild;
    }
    h->data[i] = val;
}

int compare_query_start(const void *a, const void *b)
{
    int *qa = *(int **)a;
    int *qb = *(int **)b;
    return qa[0] - qb[0];
}

int maxRemoval(int* nums, int numsSize, int** queries, int queriesSize, int* queriesColSize)
{
    qsort(queries, queriesSize, sizeof(int*), compare_query_start);

    MaxHeap available_query = { .size = 0 };
    MinHeap used_query = { .size = 0 };

    int query_pos = 0;
    int applied_count = 0;

    for (int i = 0; i < numsSize; i++)
    {
        while (query_pos < queriesSize && queries[query_pos][0] == i)
        {
            maxHeapPush(&available_query, queries[query_pos][1]);
            query_pos++;
        }

        nums[i] -= used_query.size;

        while (nums[i] > 0 && available_query.size > 0 && maxHeapTop(&available_query) >= i)
        {
            minHeapPush(&used_query, maxHeapTop(&available_query));
            maxHeapPop(&available_query);
            nums[i]--;
            applied_count++;
        }

        if (nums[i] > 0)
        {
            return -1;
        }

        while (used_query.size > 0 && minHeapTop(&used_query) == i)
        {
            minHeapPop(&used_query);
        }
    }

    return queriesSize - applied_count;
}