# PrettyConsoleHelper

A Library for Console apps that provides a prettier and easier experience with output and input parsing/validating 


Includes a InputHelper, ConsoleTable and ConsoleOutput

https://www.nuget.org/packages/PrettyConsoleHelper/
```Install-Package PrettyConsoleHelper```

# How to use

## Static classes
After adding more options and testing i have made the classes non static. But dont worry! I have wrapped the former PrettyConsole and InputHelper in a static class. Simply add this on top of your usings `using static PrettyConsoleHelper.PrettyStatic;` and now you can directly access `ConsolePretty.` and `PrettyInputHelper.`. 

### ConsolePretty
- You can still customize it by simply doing `ConsolePretty.Options = new PrettyConsoleOptions(prompt:"->");` in this case i just changed the default prompt
- `Write` and `WriteLine` methods with overloads for printing a char multiple times, printing any type and color output aswell. Default values are provided
- You can also log an Error or Warning which will print the time, message and exception (if provied) `ConsolePretty.LogError("Bad input", new ArgumentException());` [![errorlog.png](https://i.postimg.cc/nzd3ydF2/errorlog.png)](https://postimg.cc/VrC9MWb0)
- If you want your prompt with your choosen color to popup simply add `true` like this `ConsolePretty.Write("Whats your name?", true);`<br/> [![true.png](https://i.postimg.cc/bv1Q3sW8/true.png)](https://postimg.cc/d7tk0tJS)

### InputHelper with methods for validation and parsing 
- Use the Validate method in order to use attributes which you have probably seen before as data annotations. Theres also a generic overload in case you would like to parse in to a specific type. The input message is optional. `var email = PrettyInputHelper.Validate(new EmailAddressAttribute(), "Enter email: ");`<br/> [![input.png](https://i.postimg.cc/8CWCj3JK/input.png)](https://postimg.cc/5H4JrsmL)
- Having troubles parsing enums? Use the GetEnumInput() method.`var season = PrettyInputHelper.GetEnumInput<Season>();`
[![enumexample.png](https://i.postimg.cc/26wgrVBx/enumexample.png)](https://postimg.cc/vg40v8h1)


## Non static classes

### Console table
- **Options**: HeaderColor (default orange), Column Separator (default " | ")
- Expect any values being null? No worries! You can even add headers and rows with null values if you wanted to test the table format
1. Create a new table
`var table = new PrettyTable("Id", "Name", "Age");`
2. Add Rows
`foreach (var person in people)
            {
                table.AddRow(person.Id, person.Name, person);
            }`
3. Print `table.Write();`<br/>     
[![output.png](https://i.postimg.cc/wMX7tr1c/output.png)](https://postimg.cc/MfGWNdbv)

### PrettyConsole
**Options**: You can choose default coloring for lots of stuff by passing down PrettyConsoleOptions `var console = new PrettyConsole(new PrettyConsoleOptions(numberColor: ConsoleColor.Red));`
- You can use this with dependency injection. Simply add it as a singleton `services.AddSingleton<IPrettyConsole, PrettyConsole>();`

### InputHelper
**Options**: This can take in a PrettyConsole so that you are able to control the coloring and prompting!  `var console = new PrettyConsole(new PrettyConsoleOptions(numberColor: ConsoleColor.Red));
            var inputHelper = new InputHelper(console);`
            
### Dependency injection
**Options**: You can specify all the normal PrettyConsoleOptions
- InputHelper and IPrettyConsole can be dependency injected

`class Menu
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
            serviceProvider.GetRequiredService<Menu>().Run();`
[![dipretty.png](https://i.postimg.cc/1zNRTrW3/dipretty.png)](https://postimg.cc/Lq2MgLKc)

