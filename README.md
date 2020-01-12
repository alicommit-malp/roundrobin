# Round Robin implementation in C\#

![Alt Text](https://thepracticaldev.s3.amazonaws.com/i/xuyh6wviwh9geiweuqnc.jpeg)

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

Enjoy coding :)
