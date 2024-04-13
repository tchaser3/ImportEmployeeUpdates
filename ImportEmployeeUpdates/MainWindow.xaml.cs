/* Title:           Import Employee Updates
 * Date:            10-9-18
 * Author:          Terry Holmes
 * 
 * Description:     This will allow us to update employee information */

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
using System.Windows.Navigation;
using System.Windows.Shapes;
using NewEventLogDLL;
using NewEmployeeDLL;
using DepartmentDLL;
using Excel = Microsoft.Office.Interop.Excel;

namespace ImportEmployeeUpdates
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //setting up the classes
        WPFMessagesClass TheMessagesClass = new WPFMessagesClass();
        EventLogClass TheEventLogClass = new EventLogClass();
        EmployeeClass TheEmployeeClass = new EmployeeClass();
        DepartmentClass TheDepartmentClass = new DepartmentClass();

        FindSortedDepartmentDataSet TheFindSortedDepartmentDataSet = new FindSortedDepartmentDataSet();
        FindActiveEmployeesDataSet TheFindActiveEmployeesDataSet = new FindActiveEmployeesDataSet();
        EmployeeImportDataSet TheEmployeeImportDataSet = new EmployeeImportDataSet();
        FindSortedEmployeeManagersDataSet TheFindSortedEmployeeManagersDataSet = new FindSortedEmployeeManagersDataSet();
        public static UpdatesForEmployeeDataSet TheUpdatesForEmployeeDataSet = new UpdatesForEmployeeDataSet();
        FindAllActiveEmployeeInformationDataSet TheFindAllActiveEmployeeInformationDataSet = new FindAllActiveEmployeeInformationDataSet();

        public static int gintEmployeeID;
        public static string gstrFirstName;
        public static string gstrLastName;
        public static string gstrDepartment;
        public static string gstrSalaryType;
        public static string gstrManagerName;
        public static int gintManagerID;
        public static bool gblnEmployeeFound;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void mitClose_Click(object sender, RoutedEventArgs e)
        {
            TheMessagesClass.CloseTheProgram();
        }

        private void dgrResults_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void mitImportExcel_Click(object sender, RoutedEventArgs e)
        {
            Excel.Application xlDropOrder;
            Excel.Workbook xlDropBook;
            Excel.Worksheet xlDropSheet;
            Excel.Range range;

            int intColumnRange = 0;
            int intCounter;
            int intNumberOfRecords;
            string strEmployeeID;
            int intEmployeeID;
            string strFirstName;
            string strLastName;
            string strDepartment;
            string strPayID;
            int intPayID;
            string strSalaryType;
            string strManagerID;
            int intManagerID;
            string strManagerFirstName;
            string strManagerLastName;

            try
            {
                TheEmployeeImportDataSet.employeeupdate.Rows.Clear();

                Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
                dlg.FileName = "Document"; // Default file name
                dlg.DefaultExt = ".xlsx"; // Default file extension
                dlg.Filter = "Excel (.xlsx)|*.xlsx"; // Filter files by extension

                // Show open file dialog box
                Nullable<bool> result = dlg.ShowDialog();

                // Process open file dialog box results
                if (result == true)
                {
                    // Open document
                    string filename = dlg.FileName;
                }

                PleaseWait PleaseWait = new PleaseWait();
                PleaseWait.Show();

                xlDropOrder = new Excel.Application();
                xlDropBook = xlDropOrder.Workbooks.Open(dlg.FileName, 0, true, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
                xlDropSheet = (Excel.Worksheet)xlDropOrder.Worksheets.get_Item(1);

                range = xlDropSheet.UsedRange;
                intNumberOfRecords = range.Rows.Count;
                intColumnRange = range.Columns.Count;

                for (intCounter = 2; intCounter <= intNumberOfRecords; intCounter++)
                {

                    strEmployeeID = Convert.ToString((range.Cells[intCounter, 1] as Excel.Range).Value2);
                    intEmployeeID = Convert.ToInt32(strEmployeeID);
                    strFirstName = Convert.ToString((range.Cells[intCounter, 2] as Excel.Range).Value2);
                    strLastName = Convert.ToString((range.Cells[intCounter, 3] as Excel.Range).Value2);
                    strDepartment = Convert.ToString((range.Cells[intCounter, 4] as Excel.Range).Value2);
                    strPayID = Convert.ToString((range.Cells[intCounter, 5] as Excel.Range).Value2);
                    intPayID = Convert.ToInt32(strPayID);
                    strSalaryType = Convert.ToString((range.Cells[intCounter, 6] as Excel.Range).Value2);
                    strManagerID = Convert.ToString((range.Cells[intCounter, 7] as Excel.Range).Value2);
                    intManagerID = Convert.ToInt32(strManagerID);
                    strManagerFirstName = Convert.ToString((range.Cells[intCounter, 8] as Excel.Range).Value2);
                    strManagerLastName = Convert.ToString((range.Cells[intCounter, 9] as Excel.Range).Value2);
                    

                    EmployeeImportDataSet.employeeupdateRow NewEmployeeRow = TheEmployeeImportDataSet.employeeupdate.NewemployeeupdateRow();

                    NewEmployeeRow.Department = strDepartment;
                    NewEmployeeRow.EmployeeID = intEmployeeID;
                    NewEmployeeRow.FirstName = strFirstName;
                    NewEmployeeRow.LastName = strLastName;
                    NewEmployeeRow.ManagerFirstName = strManagerFirstName;
                    NewEmployeeRow.ManagerID = intManagerID;
                    NewEmployeeRow.ManagerLastName = strManagerLastName;
                    NewEmployeeRow.PayID = intPayID;
                    NewEmployeeRow.SalaryType = strSalaryType;

                    TheEmployeeImportDataSet.employeeupdate.Rows.Add(NewEmployeeRow);
                }

                PleaseWait.Close();
                dgrResults.ItemsSource = TheEmployeeImportDataSet.employeeupdate;
                mitProcess.IsEnabled = true;
            }
            catch (Exception Ex)
            {
                TheEventLogClass.InsertEventLogEntry(DateTime.Now, "Import Employee Update // Import Excel Menu Item " + Ex.Message);

                TheMessagesClass.ErrorMessage(Ex.ToString());
            }
        }
        
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            TheFindSortedEmployeeManagersDataSet = TheEmployeeClass.FindSortedEmployeeManagers();

            TheFindActiveEmployeesDataSet = TheEmployeeClass.FindActiveEmployees();
        }

        private void mitProcess_Click(object sender, RoutedEventArgs e)
        {
            //setting local veriables
            int intCounter;
            int intNumberOfRecords;
            int intEmployeeID;
            int intManagerID;
            int intPayID;
            string strDepartment;
            string strSalaryType;
            bool blnFatalError = false;

            try
            {
                intNumberOfRecords = TheEmployeeImportDataSet.employeeupdate.Rows.Count - 1;

                for(intCounter = 0; intCounter <= intNumberOfRecords; intCounter++)
                {
                    intEmployeeID = TheEmployeeImportDataSet.employeeupdate[intCounter].EmployeeID;
                    intManagerID = TheEmployeeImportDataSet.employeeupdate[intCounter].ManagerID;
                    intPayID = TheEmployeeImportDataSet.employeeupdate[intCounter].PayID;
                    strDepartment = TheEmployeeImportDataSet.employeeupdate[intCounter].Department;
                    strSalaryType = TheEmployeeImportDataSet.employeeupdate[intCounter].SalaryType;

                    blnFatalError = TheEmployeeClass.UpdateEmployeePayInformation(intEmployeeID, strDepartment, strSalaryType, intManagerID, intPayID);

                    if (blnFatalError == true)
                        throw new Exception();
                }

                TheFindAllActiveEmployeeInformationDataSet = TheEmployeeClass.FindAllActiveEmployeeInformation();

                dgrResults.ItemsSource = TheFindAllActiveEmployeeInformationDataSet.FindAllActiveEmployeeInformation;
            }
            catch (Exception Ex)
            {
                TheEventLogClass.InsertEventLogEntry(DateTime.Now, "Import Employee Updates // Main Window // Process Menu Item " + Ex.Message);

                TheMessagesClass.ErrorMessage(Ex.ToString());
            }
            
        }
    }
}
