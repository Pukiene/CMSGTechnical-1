Project Description

CMSGTechnical is a Blazor-based restaurant ordering application that allows users to browse a categorised menu, add items to a basket, and view an accurate order total including delivery fees. The application supports item quantity grouping, price ordering, persistent basket storage across page reloads, and real-time basket synchronisation between browser tabs. The system uses MediatR for request handling, DTO mapping for clean separation between domain and UI layers, and localStorage for client-side basket persistence. Unit tests have been added to validate critical mapping logic and establish measurable code coverage. This project demonstrates clean architecture principles, responsive UI behaviour, and practical handling of state management in a Blazor Server application.



CMSGTechnical – Change Log and Rationale

This document summarises the changes made to the project, the reasons for each change, and how they address the feature and bug tickets.


1. Menu Categorisation and Ordering
What changed

•	Added Category property to MenuItemDto.
•	Updated menu rendering in Home.razor to:
o	Group menu items by category.
o	Display categories in a fixed order: Starter → Main → Dessert.
o	Sort items within each category by price (ascending).

Why?

•	Required by feature ticket: “Menu items need a category, and this needs to be reflected in the UI.”
•	Fixed bug ticket: “The menu items aren't displayed in price order.”
•	Improves usability and aligns menu layout with typical restaurant structure.



2. Menu Item Display Improvements
What changed
•	Updated MenuItemDisplay.razor to display:
o	Item name before price on the same row.
o	Proper currency formatting using £ and numeric formatting.

Why

•	Fixed bug ticket: “The basket doesn't show £.”
•	Fixed UI issue where item names were not visible.
•	Ensures prices are clear, readable, and consistently formatted.

3. Chocolate Cake Description Fix
What changed

•	Added missing Description for Chocolate Cake in SeedDataHelper.SeedMenu.

Why

•	Fixed bug ticket: “Chocolate Cake's description isn't showing.”
•	Ensures all menu items have consistent descriptive content.



4. Basket Quantity Grouping
What changed

•	Updated BasketService to:
o	Group identical menu items by Id.
o	Increment Quantity instead of adding duplicate rows.

Why

Required by feature ticket: “The basket needs to group the items together using a quantity display.”
Fixed bug ticket: “The basket total doesn't add up correctly.”
Prevents duplicate entries and ensures correct totals.



5. Delivery Fee Handling
What changed

•	Added a fixed £2.00 delivery fee to the basket total calculation.
•	Displayed delivery fee and total separately in BasketDisplay.razor.

Why

•	Required by feature ticket: “The restaurant always adds £2 as a delivery fee.”
•	Improves transparency and pricing accuracy.



6. Basket Persistence and Cross-Tab Sync
What changed

•	Introduced BasketStorage using localStorage.
•	Persisted basket lines as lightweight BasketLinePersist records.
•	Implemented JavaScript storage event listener to detect cross-tab updates.
•	Reloaded basket state when another tab modifies it.


Why

•	Required by feature ticket: “The basket needs to persist between page loads.”
•	Fixed bug ticket: “When I add something to my basket on one tab, it doesn't update in another.”
•	Avoids reliance on the database for per-browser basket state.



7. Code Coverage and Testing
What changed

•	Added unit tests in CMSGTechnical.Mediator.Tests for:
o	MenuItemExtensions.ToDto
o	BasketExtensions.ToDto
•	Ensured tests execute real production code paths.

Why

Fixed issue: “Code coverage currently sits at 0%.”
Establishes a foundation for measurable coverage.
Validates correctness of DTO mapping logic.
Enables future expansion of tests for handlers and services.



8. Stability and Architecture Decisions
What changed

•	Basket persistence moved away from database dependency for UI state.
•	Database retained only for catalogue and domain entities.
•	Ensured JavaScript interop is executed only in OnAfterRenderAsync.

Why

•	Prevents server-side lifecycle issues with JS interop.
•	Simplifies basket logic and avoids unnecessary database coupling.
•	Improves reliability in Blazor Server rendering lifecycle.

