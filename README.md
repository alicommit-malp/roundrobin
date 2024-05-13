# A thread safe Round Robin implementation in C#

[Round Robin](https://en.wikipedia.org/wiki/Round-robin_scheduling) is a very simple but in the same time very useful algorithm,
but there is no native implementation in C#. So here is a simple but powerful and thread-safe implementation of the Round Robin algorithm in C\#.

<!-- > [Source Code](https://github.com/alicommit-malp/roundrobin) -->

> [Nuget](https://www.nuget.org/packages/RoundRobin/)

## Usage

```dotnet
//installation
dotnet add package RoundRobin
Install-Package RoundRobin
```

```c#
var roundRobinList = new RoundRobinList<int>(
    new List<int>{
        1,2,3,4,5
    }
);

for (var i = 0; i < 8; i++)
{
    Write($"{roundRobinList.Next()},");
}

//result
//1,2,3,4,5,1,2,3,

```

Also, you can increase/decrease the weights, by doing so you will be able to increase/decrease the priority of an element in the Round Robin list 

```c#
var roundRobinList = new RoundRobinList<int>(
    new List<int>{
        1,2,3,4,5
    }
);

//the weight of the element 1 will be increase by 2 units
roundRobinList.IncreaseWeight(element:1, amount:2);

for (var i = 0; i < 10; i++)
{
    Write($"{roundRobinList.Next()},");
}

//result
//1,1,1,2,3,4,5,1,1,1
```

Also, you can set the weights during the initialization

```c#
var roundRobinList = new RoundRobinList<int>(
    new List<int>{
        1,2,3,4,5
    },new int[]{0,1,0,0,0}
);

for (var i = 0; i < 10; i++)
{
    Write($"{roundRobinList.Next()},");
}

//result
//1,2,2,3,4,5,1,2,2,3
```



Enjoy coding :)
