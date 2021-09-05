# PrettyConsoleHelper

A small and quickly made Library for Console apps that provides a prettier and easier experience with output and input parsing/validating while helping beginners avoid tedious tasks 


Includes a InputHelper, ConsoleTable and ConsoleOutput

https://www.nuget.org/packages/PrettyConsoleHelper/
```Install-Package PrettyConsoleHelper```

# How to use

## Static classes
I have wrapped the former PrettyConsole and InputHelper in a static class. Simply add this on top of your usings
```cs
using static PrettyConsoleHelper.PrettyStatic;
``` 
now you can directly access `ConsolePretty.` and `PrettyInputHelper.` Or access the items using `PrettyStatic.`. 

### ConsolePretty
- You can still customize it by simply doing `ConsolePretty.Options = new PrettyConsoleOptions(prompt:"->");` in this case i just changed the default prompt
- `Write` and `WriteLine` methods with overloads for printing a char multiple times, printing any type and color output aswell. Default values are provided
- You can also log an Error or Warning which will print the time, message and exception (if provied) `ConsolePretty.LogError("Bad input", new ArgumentException());` [![errorlog.png](https://i.postimg.cc/nzd3ydF2/errorlog.png)](https://postimg.cc/VrC9MWb0)
- If you want your prompt with your choosen color to popup simply add `true` like this `ConsolePretty.Write("Whats your name?", true);`<br/> [![true.png](https://i.postimg.cc/bv1Q3sW8/true.png)](https://postimg.cc/d7tk0tJS)

### InputHelper with methods for validation and parsing 
- Use the Validate method in order to use attributes which you have probably seen before as data annotations. Theres also a generic overload in case you would like to parse in to a specific type. The input message is optional. 
```cs
var email = PrettyInputHelper.Validate(new EmailAddressAttribute(), "Enter email: ");
```
<br/> [![input.png](https://i.postimg.cc/8CWCj3JK/input.png)](https://postimg.cc/5H4JrsmL)
- Having troubles parsing enums? Use the GetEnumInput() method.`var season = PrettyInputHelper.GetEnumInput<Season>();`
[![enumexample.png](https://i.postimg.cc/26wgrVBx/enumexample.png)](https://postimg.cc/vg40v8h1)


## Non static classes

### Console table - fluent api style
- **Options**: HeaderColor (default orange), Column Separator (default " | ")
- Expect any values being null? No worries! You can even add headers and rows with null values if you wanted to test the table format

1. Different ways of creating a table

We have a list of person
```cs
var people = new List<Person>
            {
                new Person { Age = 50, Name = "Chris" },
                new Person { Age = 15, Name = "NotChris" }
            };
```
Fastest approach: Adds rows, headers, writes to console without storing the table in a variable
```cs
new PrettyTable()
                .AddRowsWithDefaultHeaders(people)
                .Write();
```

I recommend storing it in a variable because then you can reuse it with the ResetTable method.

*Fastest Approach*: 
```cs          
var tbl = new PrettyTable()
                .AddRowsWithDefaultHeaders(people);
```
*Fast approach*:
```cs
var table = new PrettyTable()
                .AddDefaultHeaders<Person>()
                .AddRows(people)
```

*Another fast Approach*: 
```cs
            var tbl = new PrettyTable()
                .AddHeaders("Name", "Age")
                .AddRows(people);
```

Traditional approach
```cs 
var table = new PrettyTable("Id", "Name", "Age");

foreach (var person in people)
            {
                table.AddRow(person.Id, person.Name, person);
            }
```
1. Print `table.Write();`<br/>     
[![output.png](https://i.postimg.cc/wMX7tr1c/output.png)](https://postimg.cc/MfGWNdbv)

if you would like to view the current headers in a comma separated string ``` tbl.Headers```

### PrettyConsole
**Options**: You can choose default coloring for lots of stuff by passing down PrettyConsoleOptions 
```cs
var console = new PrettyConsole(new PrettyConsoleOptions(numberColor: ConsoleColor.Red));
```

### InputHelper
**Options**: This can take in a PrettyConsole so that you are able to control the coloring and prompting!  
```cs
var console = new PrettyConsole(new PrettyConsoleOptions(numberColor: ConsoleColor.Red));
            var inputHelper = new InputHelper(console);
```
- Contains methods for taking in int input, confirming, enums, datetime input, validating input

Parse multiple arguments directly from the console using ParseOptions()
```cs
//If we have a Todo class we can do this:

  var inputHelper = new InputHelper();
            string[] options = { "-title", "-description" };
            string[] inputs = Console.ReadLine().Split(' ');

            var optionsValues = inputHelper.ParseOptions(options, inputs, "-");
            optionsValues.TryGetValue(nameof(Todo.Title), out string title);
            optionsValues.TryGetValue(nameof(Todo.Description), out string description);
```
[![example.png](https://i.postimg.cc/SNmpmyzj/example.png)](https://postimg.cc/rzZHJ2jL)
### Dependency injection
**Options**: You can specify all the normal PrettyConsoleOptions
- InputHelper and IPrettyConsole can be dependency injected

```cs
class Menu
    {
        private readonly IPrettyConsole _console;
        private readonly InputHelper _input;

        public Menu(IPrettyConsole console, InputHelper input)
        {
            _console = console;
            _input = input;
        }
        public void Run()
        {
            _console.Write("y/n", true);
            _console.WriteLine("It works!");
            var num = _input.GetIntInput("Enter a num from 5 to 10", 5, 10);
        }
    }

    class Program
    {
        static IServiceCollection ConfigureServices()
        {
            var services = new ServiceCollection();
            services.AddPrettyConsoleHelper(opt =>
            {
                opt.PromptColor = ConsoleColor.DarkGreen;
                opt.Prompt = "   >";
            });

            services.AddSingleton<Menu>();
            return services;
        }

        static void Main()
        {
            var serviceProvider = ConfigureServices().BuildServiceProvider();
            serviceProvider.GetRequiredService<Menu>().Run();
```
[![dipretty.png](https://i.postimg.cc/1zNRTrW3/dipretty.png)](https://postimg.cc/Lq2MgLKc)

