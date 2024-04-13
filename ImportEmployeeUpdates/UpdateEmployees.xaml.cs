/* Title:           Update Employee
 * Date:            10-9-18
 * Author:          Terry Holmes
 * 
 * Description:     This is used to update the employee */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using NewEmployeeDLL;
using NewEventLogDLL;
using DepartmentDLL;

namespace ImportEmployeeUpdates
{
    /// <summary>
    /// Interaction logic for UpdateEmployees.xaml
    /// </summary>
    public partial class UpdateEmployees : Window
    {
        WPFMessagesClass TheMessagesClass = new WPFMessagesClass();
        EmployeeClass TheEmployeeClass = new EmployeeClass();
        EventLogClass TheEventLogClass = new EventLogClass();
        DepartmentClass TheDepartmentClass = new DepartmentClass();

        //setting up the data
        FindSortedDepartmentDataSet TheFindSortedDepartmentDataSet = new FindSortedDepartmentDataSet();
        FindAllEmployeesByLastNameDataSet TheFindAllEmployeesByLastNameDataSet = new FindAllEmployeesByLastNameDataSet();
        FindSortedEmployeeManagersDataSet TheFindSortedEmployeeManagersDataSet = new FindSortedEmployeeManagersDataSet();
        FindWarehousesDataSet TheFindWarehousesDataSet = new FindWarehousesDataSet();
        FindEmployeeByEmployeeIDDataSet TheFindEmployeeByEmployeeIDDataSet = new FindEmployeeByEmployeeIDDataSet();

        public UpdateEmployees()
        {
            InitializeComponent();
        }

        private void cboSelectEmployee_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int intSelectedIndex;
            int intCounter;
            int intNumberOfRecords;
            bool blnActive;
            string strEmployeeType;
            string strHomeOffice;
            string strGroup;
            int intComboSelectedIndex = 0;
            string strSalaryType;
            string strDepartment;
            int intManagerID;


            try
            {
                intSelectedIndex = cboSelectEmployee.SelectedIndex - 1;

                if (intSelectedIndex > -1)
                {
                    MainWindow.gintEmployeeID = TheFindAllEmployeesByLastNameDataSet.FindAllEmployeeByLastName[intSelectedIndex].EmployeeID;

                    TheFindEmployeeByEmployeeIDDataSet = TheEmployeeClass.FindEmployeeByEmployeeID(MainWindow.gintEmployeeID);

                    txtEmployeeID.Text = Convert.ToString(MainWindow.gintEmployeeID);
                    txtFirstName.Text = TheFindEmployeeByEmployeeIDDataSet.FindEmployeeByEmployeeID[0].FirstName;
                    txtLastName.Text = TheFindEmployeeByEmployeeIDDataSet.FindEmployeeByEmployeeID[0].LastName;
                    txtPhoneNumber.Text = TheFindEmployeeByEmployeeIDDataSet.FindEmployeeByEmployeeID[0].PhoneNumber;

                    blnActive = TheFindAllEmployeesByLastNameDataSet.FindAllEmployeeByLastName[intSelectedIndex].Active;
                    strEmployeeType = TheFindAllEmployeesByLastNameDataSet.FindAllEmployeeByLastName[intSelectedIndex].EmployeeType;
                    strHomeOffice = TheFindAllEmployeesByLastNameDataSet.FindAllEmployeeByLastName[intSelectedIndex].HomeOffice;
                    strGroup = TheFindAllEmployeesByLastNameDataSet.FindAllEmployeeByLastName[intSelectedIndex].EmployeeGroup;

                    if (TheFindAllEmployeesByLastNameDataSet.FindAllEmployeeByLastName[intSelectedIndex].IsEmailAddressNull() == false)
                    {
                        txtEmailAddress.Text = TheFindAllEmployeesByLastNameDataSet.FindAllEmployeeByLastName[intSelectedIndex].EmailAddress;
                    }
                    else
                    {
                        txtEmailAddress.Text = "";
                    }
                    if (TheFindAllEmployeesByLastNameDataSet.FindAllEmployeeByLastName[intSelectedIndex].IsDepartmentNull() == true)
                    {
                        strDepartment = "";

                        cboSelectDepartment.SelectedIndex = 0;
                    }
                    else
                    {
                        strDepartment = TheFindAllEmployeesByLastNameDataSet.FindAllEmployeeByLastName[intSelectedIndex].Department;

                        for (intCounter = 0; intCounter <= cboSelectDepartment.Items.Count - 1; intCounter++)
                        {
                            if (strDepartment == cboSelectDepartment.SelectedItem.ToString())
                            {
                                cboSelectDepartment.SelectedIndex = intCounter;
                            }
                        }
                    }

                    if (TheFindAllEmployeesByLastNameDataSet.FindAllEmployeeByLastName[intSelectedIndex].IsManagerIDNull() == true)
                    {
                        intManagerID = 0;

                        cboSelectManager.SelectedIndex = 0;
                    }
                    else if (TheFindAllEmployeesByLastNameDataSet.FindAllEmployeeByLastName[intSelectedIndex].ManagerID == 0)
                    {
                        intManagerID = 0;

                        cboSelectManager.SelectedIndex = 0;
                    }
                    else
                    {
                        intManagerID = TheFindAllEmployeesByLastNameDataSet.FindAllEmployeeByLastName[intSelectedIndex].ManagerID;

                        intNumberOfRecords = TheFindSortedEmployeeManagersDataSet.FindSortedEmployeeManagers.Rows.Count - 1;

                        for (intCounter = 0; intCounter <= intNumberOfRecords; intCounter++)
                        {
                            if (intManagerID == TheFindSortedEmployeeManagersDataSet.FindSortedEmployeeManagers[intCounter].employeeID)
                            {
                                cboSelectManager.SelectedIndex = intCounter + 1;
                            }
                        }
                    }

                    if (TheFindAllEmployeesByLastNameDataSet.FindAllEmployeeByLastName[intSelectedIndex].IsSalaryTypeNull() == true)
                    {
                        strSalaryType = "";

                        cboSelectSalaryType.SelectedIndex = 0;
                    }
                    else
                    {
                        strSalaryType = TheFindAllEmployeesByLastNameDataSet.FindAllEmployeeByLastName[intSelectedIndex].SalaryType;

                        for (intCounter = 0; intCounter < cboSelectSalaryType.Items.Count; intCounter++)
                        {
                            cboSelectSalaryType.SelectedIndex = intCounter;

                            if (strSalaryType == cboSelectSalaryType.SelectedItem.ToString())
                            {
                                break;
                            }
                        }
                    }
                    if (TheFindAllEmployeesByLastNameDataSet.FindAllEmployeeByLastName[intSelectedIndex].IsDepartmentNull() == true)
                    {
                        cboSelectDepartment.SelectedIndex = 0;
                    }
                    else
                    {
                        strDepartment = TheFindAllEmployeesByLastNameDataSet.FindAllEmployeeByLastName[intSelectedIndex].Department;

                        for (intCounter = 0; intCounter < cboSelectDepartment.Items.Count; intCounter++)
                        {
                            cboSelectDepartment.SelectedIndex = intCounter;

                            if (strDepartment == cboSelectDepartment.SelectedItem.ToString())
                            {
                                break;
                            }
                        }
                    }

                    if (blnActive == true)
                        cboSelectActive.SelectedIndex = 1;
                    else
                        cboSelectActive.SelectedIndex = 2;

                    intNumberOfRecords = cboSelectGroup.Items.Count - 1;

                    for (intCounter = 0; intCounter <= intNumberOfRecords; intCounter++)
                    {
                        cboSelectGroup.SelectedIndex = intCounter;

                        if (cboSelectGroup.SelectedItem.ToString() == strGroup)
                        {
                            intComboSelectedIndex = intCounter;
                        }
                    }

                    cboSelectGroup.SelectedIndex = intComboSelectedIndex;

                    intNumberOfRecords = cboSelectHomeOffice.Items.Count - 1;

                    for (intCounter = 0; intCounter <= intNumberOfRecords; intCounter++)
                    {
                        cboSelectHomeOffice.SelectedIndex = intCounter;

                        if (cboSelectHomeOffice.SelectedItem.ToString() == strHomeOffice)
                        {
                            intComboSelectedIndex = intCounter;
                        }
                    }

                    cboSelectHomeOffice.SelectedIndex = intComboSelectedIndex;

                    intNumberOfRecords = cboSelectEmployeetype.Items.Count - 1;

                    for (intCounter = 0; intCounter <= intNumberOfRecords; intCounter++)
                    {
                        cboSelectEmployeetype.SelectedIndex = intCounter;

                        if (cboSelectEmployeetype.SelectedItem.ToString() == strEmployeeType)
                        {
                            intComboSelectedIndex = intCounter;
                        }
                    }

                    cboSelectEmployeetype.SelectedIndex = intComboSelectedIndex;
                }
            }
            catch (Exception Ex)
            {
                TheEventLogClass.InsertEventLogEntry(DateTime.Now, "Import Employee Updates // Update Employee // Select Employee Combo Box " + Ex.Message);

                TheMessagesClass.ErrorMessage(Ex.ToString());
            }
        }

        private void cboSelectActive_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void cboSelectGroup_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void cboSelectHomeOffice_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void cboSelectEmployeetype_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void cboSelectSalaryType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void cboSelectDepartment_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void cboSelectManager_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //this will call the like search
            int intNumberOfRecords;
            int intCounter;
            int intLength;

            try
            {
                intLength = MainWindow.gstrLastName.Length;

                if (intLength > 2)
                {
                    cboSelectEmployee.Items.Clear();
                    cboSelectEmployee.Items.Add("Select Employee");

                    TheFindAllEmployeesByLastNameDataSet = TheEmployeeClass.FindAllEmployeesByLastName(MainWindow.gstrLastName);

                    intNumberOfRecords = TheFindAllEmployeesByLastNameDataSet.FindAllEmployeeByLastName.Rows.Count - 1;

                    if (intNumberOfRecords == -1)
                    {
                        TheMessagesClass.ErrorMessage("Employees Not Found");
                        return;
                    }

                    for (intCounter = 0; intCounter <= intNumberOfRecords; intCounter++)
                    {
                        cboSelectEmployee.Items.Add(TheFindAllEmployeesByLastNameDataSet.FindAllEmployeeByLastName[intCounter].FirstName + " " + TheFindAllEmployeesByLastNameDataSet.FindAllEmployeeByLastName[intCounter].LastName);
                    }

                    cboSelectEmployee.SelectedIndex = 0;
                }
            }
            catch (Exception Ex)
            {
                TheEventLogClass.InsertEventLogEntry(DateTime.Now, "Importedd Employee Update // Edit Employee // Enter Last Name Event " + Ex.Message);

                TheMessagesClass.ErrorMessage(Ex.ToString());
            }
        }

        private void mitClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}
