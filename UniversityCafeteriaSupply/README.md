# University Cafeteria Supply Request Management System

Simple ASP.NET Core MVC solution for the attached assignment (no database needed).

## Run
1. Open `UniversityCafeteriaSupply.csproj` in Visual Studio 2022 (ASP.NET and web development workload), or open the folder in VS Code.
2. Install the .NET 8 SDK if needed.
3. Run `dotnet restore`, then `dotnet run` in this folder.
4. Open the `http://localhost:...` or `https://localhost:...` URL displayed in the terminal.

## How it works
- **Create Supply Request**: Enter a product, category, quantity, price and priority; add more items with the same form.
- **Review Draft**: Shows all items, subtotal, preparation fee, discount and final total. Confirm from here.
- **My Requests**: View confirmed requests.
- **Clear Draft**: Delete draft products from Session.
- **Remember My Priority**: Saves the preferred priority in a cookie for 7 days.

## Important notes
- The draft uses ASP.NET Core Session for 30 minutes of inactivity.
- Confirmed requests are stored in an in-memory static List (as the assignment asks), and **are lost when the application restarts**. This is a learning example, not permanent storage or per-user isolation.
- To keep the example simple, the **latest added product's priority** determines the overall request preparation fee. Each product also keeps its own chosen priority.
- Fee: Normal 2 JD, Urgent 5 JD, Emergency 10 JD.
- Discount: subtotal > 200 JD = 15%, else subtotal > 100 JD = 10%, else 0%.
- If you are adding this task to an existing Cafeteria project, copy `SupplyRequestController.cs`, `SupplyModels.cs`, the `Views/SupplyRequest` folder and the layout links. Also configure Session in `Program.cs`. The `CafeteriaController` and its pages in this ZIP are basic placeholders for standalone running.
