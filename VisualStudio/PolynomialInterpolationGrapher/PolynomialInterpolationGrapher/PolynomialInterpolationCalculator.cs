using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PolynomialInterpolationGrapher
{
    /*
    This class takes in a set of (x,y) points as a csv string, and creates
    a Lagrange interpolation polynomial that goes through all those points.
    Gives the interpolation polynomial in the form of a string.
    */
    public class PolynomialInterpolationCalculator
    {
        private string csvPoints;

        /*
        The default constructor (shouldn't really be used at all).
        */
        public PolynomialInterpolationCalculator()
        {
            csvPoints = "";
        }

        /*
        The constructor we'll usually use. Takes in a string of csv (x,y) coordinates.
        */
        public PolynomialInterpolationCalculator(string csvPts)
        {
            csvPoints = csvPts;
        }
        
        /*
        This function is the main function of our class. It takes the csv string that we created
        the class with and creates a Lagrange polynomial string to return.
        */
        public string getLagrangePolynomial()
        {
            List<string> xPoints = new List<string>();
            List<string> yPoints = new List<string>();

            //First get the (x,y) values from our csv string
            for (int i = 0; i < csvPoints.Length; i++)
            {
                if (csvPoints[i] == '(')
                {
                    string tempString = "";
                    int j = i + 1;
                    //Get the x point
                    for (; csvPoints[j] != ','; j++)
                        tempString += csvPoints[j];
                    xPoints.Add(tempString);
                    ++j;
                    tempString = "";
                    //Get the y point
                    for (; csvPoints[j] != ')'; j++)
                        tempString += csvPoints[j];
                    yPoints.Add(tempString);
                    //Jump i ahead to the current point
                    i = j;
                }
            }
            
            //Next, get everything into lagrange form
            string lagrangeString = "";
            if (yPoints.Count > 1)
            {
                for (int i = 0; i < yPoints.Count; i++)
                {
                    //First, the y value
                    lagrangeString += "(" + yPoints[i] + ")*";
                    //Next all the x values
                    string numerator = "";
                    string denominator = "";
                    for (int j = 0; j < xPoints.Count; j++)
                    {
                        if (j == i)
                            continue;
                        numerator += "(x-" + "(" + xPoints[j] + ")" + ")";
                        denominator += "(" + xPoints[i] + "-" + "(" + xPoints[j] + ")" + ")";
                        if (i < yPoints.Count - 1)
                        {
                            if (j < xPoints.Count - 1)
                            {
                                numerator += "*";
                                denominator += "*";
                            }
                        }
                        else
                        {
                            if (j < xPoints.Count - 2)
                            {
                                numerator += "*";
                                denominator += "*";
                            }
                        }
                    }
                    //Now add the combo to the Lagrange string
                    lagrangeString += "(" + numerator + ")" + "/" + "(" + denominator + ")";
                    if (i < yPoints.Count - 1)
                        lagrangeString += "+";
                }
            }
            else
            {
                lagrangeString = "(" + yPoints[0] + ")";
            }

            //Finally, return the lagrange string
            return lagrangeString;
        }
    }
}
