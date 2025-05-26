<h1 align="center">
  Назва: KotikiShop
</h1>
<p align="center">
  <img src="KotikiShop/wwwroot/images/icon.png" alt="Logo">
</p>
<h1 align="center">
  Мета
</h1> 
Розробити сайт з великим функціоналом, красивим дизайном та зручним інтерфейсом.
<h1 align="center">
  Користувачі та їх можливості
</h1>
<h2>
  Guest - гість
</h2>
Має можливість дивитись каталог, для додаткових можливостей повинен зареєструватися.
<h2>
  Customer - покупець
</h2>
Має можливості дивитись каталог, кошик своїх покупок, додавати котів в кошик та робити замовлення. Також має можливість дивитися та редагувати дані свого облікового запису.
<h2>
  Admin - адмін
</h2>
Має всі можливості покупця, також може редагувати пропозиції по продажу котів, та редагувати каталог.
<h1 align="center">
  Основні сутності (основні об'єкти, якими оперує застосунок, та їх поля)
</h1>
Cat - має такі характеристики: ім'я, опис, вік, порода, ціна, стать<br>
CatFamily - порода<br>
Application User - користувач<br>
CatComment - не дороблено<br>
Cart - кошик покупок<br>
CartItem - об'єкт в кошику
<h1 align="center">
  Можливості застосунку
</h1>
Якщо хочете знайти собі лагідного та ніжного породистого котика то вам до нас! Наш сайт пропонує наступні можливості:<br>
Каталог котиків та їх фільтр по ціні, породі та віку<br>
Пошук по вашому запиту<br>
Зручний кошик покупок<br>
<h1 align="center">
  Вибір технології UI
</h1>
ASP.NET MVC
<h3>
  Ключові переваги:
</h3>
Розподіл обов’язків – архітектура MVC (Model-View-Controller) дозволяє чітко розділити логіку додатка, що спрощує підтримку та тестування.<br>
Гнучкість у розробці – можна легко налаштовувати компоненти, використовувати різні бібліотеки JavaScript (наприклад, jQuery, React, Vue).<br>
Вбудована підтримка Razor – ефективний механізм створення динамічних HTML-сторінок.<br>
Висока продуктивність – завдяки кешуванню та оптимізованому рендерингу сторінок.<br>
Легка інтеграція з .NET – дозволяє використовувати потужні можливості .NET (наприклад, Entity Framework, LINQ).<br>
SEO-дружність – підтримка чистих URL, що важливо для пошукової оптимізації.<br>
Безпека – вбудований захист від CSRF, XSS, SQL-ін’єкцій.

<h1 align="center">
  Як запустити цей проєкт (для повних чайників)
</h1>

### Що потрібно встановити перед початком

1. **Visual Studio 2022**  
   Завантажити: [https://visualstudio.microsoft.com/](https://visualstudio.microsoft.com/)  
   Під час встановлення **обов'язково вибери робоче навантаження**:
   - **ASP.NET and web development**
   - **.NET desktop development**
   - **Data storage and processing**

2. **SQL Server** (якщо не встановлено)  
   Рекомендується **SQL Server 2022 Developer**  
   Завантажити: [https://www.microsoft.com/en-us/sql-server/sql-server-downloads](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)

3. **SQL Server Management Studio (SSMS)** — для зручної роботи з базою  
   Завантажити: [https://learn.microsoft.com/en-us/ssms/download-sql-server-management-studio-ssms](https://learn.microsoft.com/en-us/ssms/download-sql-server-management-studio-ssms)

### Як завантажити проєкт

1. Відкрий GitHub-репозиторій
2. Натисни кнопку `Code` → `Download ZIP` або скористайся Git:
   ```bash
   git clone https://github.com/Zefir-13000/KotikiShop.git
   ```
3. Розпакуй або відкрий репозиторій у **Visual Studio 2022**:
   - `File` → `Open` → `Project/Solution...` → вибери `.sln` файл

### Налаштування бази даних

1. Відкрий файл `appsettings.json`
2. Знайди рядок з `ConnectionStrings` і переконайся, що вказано правильне з'єднання. Наприклад:
   ```json
      "DefaultConnection": "Server=YOUR_SERVER_NAME;Database=KotikiShop;Trusted_Connection=True;TrustServerCertificate=True;Encrypt=False",
      "ApplicationDbContextConnection": "Server=(localdb)\\mssqllocaldb;Database=KotikiShop;Trusted_Connection=True;MultipleActiveResultSets=true"
   ```

### Створення бази даних через Entity Framework

1. Відкрий **Package Manager Console**:
   - `Tools` → `NuGet Package Manager` → `Package Manager Console`

2. Виконай команду:
   ```powershell
   Update-Database
   ```

   Якщо є помилки:
   - Перевір правильність `Default Project` у меню Package Manager Console(Потрібно обрати `KotikiShop.DataAccess`)
   - Перевір, що SQL Server запущено

### Запуск проєкту

1. У Visual Studio натисни зелену кнопку **"Start"** або клавішу **F5**(Перед цим оберіть поруч з зеленою стрілкою веб-сервер `IIS Express`)
2. Додаток відкриється в браузері зазвичай на `https://localhost:xxxx`


### Поширені проблеми

- **"Cannot connect to SQL Server"**  
  → Перевір, чи запущено SQL Server і правильний рядок підключення в `appsettings.json`

- **"No migration configured"**  
  → Спробуй команду(в `Package Manager Console`):
  ```powershell
  Add-Migration Initial
  ```
  а потім:
  ```powershell
  Update-Database
  ```

- **Помилка SSL / сертифіката в браузері**  
  → Просто погодься на ризик і продовж, це нормально для localhost


### Все готово!

Тепер ти можеш користуватись проєктом! Якщо щось не працює — спробуй перечитати інструкцію повільно ще раз :)
