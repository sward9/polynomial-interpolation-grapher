using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using org.mariuszgromada.math.mxparser;


namespace PolynomialInterpolationGrapher
{
    /*
    This form is the user interface for our polynomial interpolation graphing.
    It pretty much handles everything.
    */
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        //This function finds the string of a x coordinate in a given coordinate pair string
        string findXCoordinate(string coordinatePair)
        {
            string tempString = "";
            for(int i = 1; i < coordinatePair.Length && coordinatePair[i] != ','; i++)
                tempString += coordinatePair[i];
            return tempString;
        }
        //This function finds the string of a y coordinate in a given coordinate pair string
        string findYCoordinate(string coordinatePair)
        {
            string tempString = "";
            for (int i = 1; i < coordinatePair.Length; i++)
                if (coordinatePair[i] == ',')
                    for (int j = i + 1; j < coordinatePair.Length && coordinatePair[j] != ')'; j++)
                        tempString += coordinatePair[j];
            return tempString;
        }
        //This function finds the greatest x coordinate (absolute) of a given csv of coordinate points
        double findGreatestX(string coordinateCSV)
        {
            string coordinates = coordinateCSV.Replace("),", ")").Replace(",(", "(");
            double greatestX = 0;
            List<string> coordinatesList = new List<string>();
            string tempString = "";
            for(int i = 0; i < coordinates.Length; i++)
            {
                tempString += coordinates[i];
                if (coordinates[i] == ')')
                {
                    coordinatesList.Add(tempString);
                    tempString = "";
                }
            }
            foreach(string coordinate in coordinatesList)
            {
                double testCoordinate = Math.Abs(Double.Parse(findXCoordinate(coordinate)));
                if (testCoordinate > greatestX)
                    greatestX = testCoordinate;
            }
            return greatestX;
        }
        //This function removes leading zeros from a "number" (a string representing a number)
        string removeLeadingZeros(string number)
        {
            if (number.Length < 2)
                return number;
            else if (number[1] == '.')
                return number;
            int firstZero = 0;
            for (int i = 0; i < number.Length - 1 && number[i] == '0' && number[i+1] != '.'; i++)
                firstZero += 1;
            string tempString = "";
            for (int i = firstZero; i < number.Length; i++)
                tempString += number[i];
            return tempString;
        }
        //This function removes trailing zeros from a "number" (a string representing a number)
        string removeTrailingZeros(string number)
        {
            int decimalPlaceIndex = -1;
            for(int i = 0; i < number.Length; i++)
            {
                if(number[i] == '.')
                {
                    decimalPlaceIndex = i;
                    break;
                }
            }
            if (decimalPlaceIndex == -1)
                return number;
            int trailingZerosEndIndex = -1;
            for(int i = number.Length - 1; i > decimalPlaceIndex; i--)
            {
                if(number[i] != '0')
                {
                    trailingZerosEndIndex = i;
                    break;
                }
            }
            if (trailingZerosEndIndex == -1)
                trailingZerosEndIndex = decimalPlaceIndex - 1;
            string tempString = "";
            for (int i = 0; i <= trailingZerosEndIndex; i++)
                tempString += number[i];
            return tempString;
        }
        /*
        This function evaluates a string function with a given value for every "x" character in the function. 
        */
        double evaluateWithX(string func, double x)
        {
            //The expression interpreter is https://github.com/dynamicexpresso/DynamicExpresso
            string editedFunc = func.Replace("x", "(" + x.ToString() + ")");

            Expression e = new Expression(editedFunc);

            return e.calculate();
        }
        /*
        This function creates the data points for our graphable function.
        Got help from https://stackoverflow.com/questions/25313258/how-do-i-graph-a-custom-function-in-oxyplot/28960073
        */
        FunctionSeries getGraphableFunction(string func, double significantX)
        {
            FunctionSeries fSer = new FunctionSeries();
            double n = (double)Math.Floor(significantX);
            for (double x1 = -n; x1 < n; x1++)
            {
                for (double x2 = x1; x2 <= x1 + 1; x2 += 0.1)
                {
                    DataPoint dp = new DataPoint(x2, evaluateWithX(func, x2));
                    fSer.Points.Add(dp);
                }
            }
            return fSer;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if(textBox1.Text.Length > 0)
            { 
                string textBoxText = textBox1.Text;
                //Make sure that we're only inputting numbers or periods
                Regex rgx = new Regex("[^0-9.-]");
                textBox1.Text = rgx.Replace(textBoxText, "");
                if (textBox1.Text != "")
                {
                    //Make sure there's only one decimal point
                    bool foundFirstDecimalPlace = false;
                    for (int i = 0; i < textBoxText.Length; i++)
                    {
                        if (textBoxText[i] == '.')
                        {
                            if (foundFirstDecimalPlace)
                            {
                                string tempString = "";
                                for (int j = 0; j < i; j++)
                                    tempString += textBoxText[j];
                                textBox1.Text = tempString;
                            }
                            else
                            {
                                foundFirstDecimalPlace = true;
                            }
                        }
                    }
                    //Make sure that if we entered a minus sign, it's only gonna be the first character of the string
                    string tempString2 = "";
                    tempString2 += textBox1.Text[0];
                    for (int i = 1; i < textBox1.Text.Length; i++)
                        if (textBox1.Text[i] != '-')
                            tempString2 += textBox1.Text[i];
                    textBox1.Text = tempString2;
                    //Finally, if our entry is just a decimal point, put a zero before it
                    if (textBox1.Text == ".")
                        textBox1.Text = "0.";
                }
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (textBox2.Text.Length > 0)
            {
                string textBoxText = textBox2.Text;
                //Make sure that we're only inputting numbers or periods
                Regex rgx = new Regex("[^0-9.-]");
                textBox2.Text = rgx.Replace(textBoxText, "");
                if (textBox2.Text != "")
                {
                    //Make sure there's only one decimal point
                    bool foundFirstDecimalPlace = false;
                    for (int i = 0; i < textBoxText.Length; i++)
                    {
                        if (textBoxText[i] == '.')
                        {
                            if (foundFirstDecimalPlace)
                            {
                                string tempString = "";
                                for (int j = 0; j < i; j++)
                                    tempString += textBoxText[j];
                                textBox2.Text = tempString;
                            }
                            else
                            {
                                foundFirstDecimalPlace = true;
                            }
                        }
                    }
                    //Make sure that if we entered a minus sign, it's only gonna be the first character of the string
                    string tempString2 = "";
                    tempString2 += textBox2.Text[0];
                    for (int i = 1; i < textBox2.Text.Length; i++)
                        if (textBox2.Text[i] != '-')
                            tempString2 += textBox2.Text[i];
                    textBox2.Text = tempString2;
                    //Finally, if our entry is just a decimal point, put a zero before it
                    if (textBox2.Text == ".")
                        textBox2.Text = "0.";
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //First, make sure that we have a value in each of the X and Y text boxes
            if(textBox1.Text.Length > 0 && textBox2.Text.Length > 0)
            {                
                //Next, make sure a point with the given x coordinate doesn't already exist
                bool xCoordinateAlreadyExists = false;
                foreach(string item in listBox1.Items)
                {
                    if(removeLeadingZeros(removeTrailingZeros(textBox1.Text)) == findXCoordinate(item))
                    {
                        xCoordinateAlreadyExists = true;
                        break;
                    }
                }
                if (xCoordinateAlreadyExists)
                {
                    MessageBox.Show("A point with the given X coordinate already exists.");
                }                
                //Finally, if all the tests succeeded, at the coordinate to the item list
                else
                {
                    string coordinatePoint = "(" + removeLeadingZeros(removeTrailingZeros(textBox1.Text)) + "," + removeLeadingZeros(removeTrailingZeros(textBox2.Text)) + ")";
                    listBox1.Items.Add(coordinatePoint);
                }
            }
            else
            {
                MessageBox.Show("Please enter both an X and a Y coordinate.");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //Reset the graph
            listBox1.Items.Clear();
            plotView1.Model = null;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //If we haven't added any points, then give an error
            if (listBox1.Items.Count < 1)
            {
                MessageBox.Show("Please add at least one point to graph.");
            }
            //Else, construct a Lagrange polynomial
            else
            {
                //First, create a string of all the points
                string points = "";
                foreach (string point in listBox1.Items)
                {
                    points += point + ",";
                }                
                //Then, calculate the Lagrange interpolation polynomial based on all these points
                PolynomialInterpolationCalculator pic = new PolynomialInterpolationCalculator(points);
                string polynomialString = pic.getLagrangePolynomial();
                //Next, find the significant maximum x to show on the graph for our function
                double significantX = findGreatestX(points) + 10;
                //Graph the polynomial using Oxyplot               
                PlotModel plotModel = new PlotModel { Title = "" };
                plotModel.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom});
                plotModel.Axes.Add(new LinearAxis { Position = AxisPosition.Left});
                plotModel.Series.Add(getGraphableFunction(polynomialString, significantX));
                foreach (string point in listBox1.Items)
                {
                    OxyPlot.Annotations.PointAnnotation tempPoint = new OxyPlot.Annotations.PointAnnotation();
                    tempPoint.X = double.Parse(findXCoordinate(point));
                    tempPoint.Y = double.Parse(findYCoordinate(point));
                    tempPoint.Text = point;
                    plotModel.Annotations.Add(tempPoint);
                }
                plotView1.Model = plotModel;
            }
        }
    }
}
