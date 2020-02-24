# Selenium-Functional-Tests #
A test automation project built on xunit framework using .NET Core and C#.

### Geophy Showcase Test ###
This repository contains a sample test automation project built on xunit framework using .NET Core and C#. The tests have been split into two separate suites with 5 test cases in the SmokeTests Suite and 10 test cases in the Regression Suite.
Please see description of tests below:

**SmokeTest Suite:**
*TC-001: Verify_RunValuation_Details()
*TC-002: Verify_RunValuation_ModifyInputDetails()
*TC-003: Verify_SignUp()
*TC-004: Verify_Login()
*TC-005: Verify_Logout()

**Regression Suite:**
*TC-006: Verify_Login_WithoutEmail()
*TC-007: Verify_Login_WithoutPassword()
*TC-008: Verify_Login_WithUnverifiedEmail()
*TC-009: Verify_Login_WithIncorrectDetails()
*TC-010: Verify_SignUp_WithoutEmail()
*TC-011: Verify_SignUp_WithoutPassword()
*TC-012: Verify_SignUp_WithoutFirstName()
*TC-013: Verify_SignUp_WithoutLastName()
*TC-014: Verify_SignUp_WithoutCheckBoxTandC()
*TC-015: Verify_SignUp_WithRegisteredEmail()

**Architecture of Geophy Showcase Test - (A Selenium .NET Core Project)**
<kbd>Right</kbd> Page object model using the PageObjects Class
<kbd>Right</kbd> Data driven approach through a JSon file and using the ConfigurationBuilder Class
<kbd>Right</kbd> The same driver instance context is shared betwwen all tests using the IClassFixture class Single instance of driver used for all tests
<kbd>Right</kbd> Screenshot implementation with custom file svaed to a dedicated directory


*Prerequisites:*
*.NET Core SDK v3.1 or higher

*Development Environment:*
On any terminal (I used Git Bash) move to the "Geophy.Tests" folder (the folder containing the "Geophy.Tests.csproj" file), and execute the following commands:

*dotnet restore
*dotnet test

Alternatively, you can open the project in Visual Studio or VS Code IDE, build the project then run/execute the test.

*Happy Testing* :smiley:
