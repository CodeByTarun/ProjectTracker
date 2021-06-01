<!-- Project Logo -->
<p align="center">
  <h1 align="center">Project Tracker</h1>
  <p align="center">Keep track of your projects with this easy-to-use kanban application!</p>
</p>

<!-- Table of Contents -->
<details open="open">
  <summary>Table of Contents</summary>
  <ol>
    <li>
      <a href="#about-the-project">About The Project</a>
      <ul>
        <li><a href="#built-with">Built With</a></li>
      </ul>
    </li>
    <li>
      <a href="#getting-started">Getting Started</a>
    </li>
    <li><a href="#usage">Usage</a></li>
    <li><a href="#roadmap">Roadmap</a></li>
    <li><a href="#contributing">Contributing</a></li>
    <li><a href="#license">License</a></li>
    <li><a href="#contact">Contact</a></li>
    <li><a href="#acknowledgements">Acknowledgements</a></li>
  </ol>
</details>

## About The Project

![image](https://user-images.githubusercontent.com/14295466/119246293-70d85100-bb4e-11eb-95e0-7907fef3961b.png)

Project tracker was created because I wanted an easy-to-use lightweight Kanban board application to keep track of projects that I was working on. Also, it was a way for me to showcase a lot of the programming concepts and skills I have developed. A detailed explanation of how to use the application is in the [Usage](https://github.com/TarunBola/ProjectTracker#usage) section below. 

This WPF application was created using the Model-view-viewmodel (MVVM) design pattern. The separation this pattern provides between the UI and business logic made it easier to make changes to the application. Many other design patterns were used to create this project such as the singleton pattern, composite pattern, command pattern, observer pattern, factory method pattern, etc. IOC containers were used as well for easier incorporation of dependency injection.

 All the service classes were created through test driven development (TTD) which saved a lot of time when the service classes dealt with concepts such as linked lists in SQL and SQL queries. The database was created using the Code First approach in Entity Framework. This allowed for creation of the model in C# first and then through configurations of each class in the model, a database was created to match it.  

### Built With
* [.NET Core](https://dotnet.microsoft.com/)
* [WPF (Windows Presentation Foundation)](https://docs.microsoft.com/en-us/dotnet/desktop/wpf/?view=netdesktop-5.0)
* [Entity Framework Core](https://docs.microsoft.com/en-us/ef/)

## Getting Started

## Usage

## Roadmap

See the [open issues](https://github.com/TarunBola/ProjectTracker/issues) for a list of proposed features (and known issues).

## Contributing

Any contributions to this project are welcomed! To contribute:

1. Fork the Project
2. Create your Feature Branch (`git checkout -b feature/AmazingFeature`)
3. Commit your Changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the Branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

## License

Distributed under the [MIT License](https://github.com/othneildrew/Best-README-Template/blob/master/LICENSE.txt).

## Contact
Tarun Bola - Tarundeep1@hotmail.com

Project Link: [https://github.com/TarunBola/ProjectTracker](https://github.com/TarunBola/ProjectTracker)

## Acknowledgments
* [Extended WPF Toolkit](https://github.com/xceedsoftware/wpftoolkit)
* [XAML Behaviours WPF](https://github.com/Microsoft/XamlBehaviorsWpf)
* [Font Awesome](https://fontawesome.com)
* [Icons8](https://icons8.com/)

<!-- Links and Images -->
[license-url]: https://github.com/othneildrew/Best-README-Template/blob/master/LICENSE.txt
