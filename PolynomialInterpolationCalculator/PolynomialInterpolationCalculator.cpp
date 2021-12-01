#include "PolynomialInterpolationCalculator.h"

#include <vector>
#include <string>

using namespace std;

/*
The default constructor (shouldn't really be used at all).
*/
PolynomialInterpolationCalculator::PolynomialInterpolationCalculator(){
    csvPoints = "";
}

/*
The constructor we'll usually use. Takes in a string of csv (x,y) coordinates.
*/
PolynomialInterpolationCalculator::PolynomialInterpolationCalculator(string csvPts){
    csvPoints = csvPts;
}

/*
This function is the main function of our class. It takes the csv string that we created
the class with and creates a Lagrange polynomial string to return.
*/
string PolynomialInterpolationCalculator::getLagrangePolynomial(){
    vector<string> xPoints;
    vector<string> yPoints;

    //First get the (x,y) values from our csv string
    for(unsigned int i = 0; i < csvPoints.length(); i++){
        if(csvPoints.at(i) == '('){
            string tempString = "";
            unsigned int j = i + 1;
            //Get the x point
            for(; csvPoints.at(j) != ','; j++)
                tempString += csvPoints.at(j);
            xPoints.push_back(tempString);
            ++j;
            tempString = "";
            //Get the y point
            for(; csvPoints.at(j) != ')'; j++)
                tempString += csvPoints.at(j);
            yPoints.push_back(tempString);
            //Jump i ahead to the current point
            i = j;
        }
    }

    //Next, get everything into lagrange form
    string lagrangeString = "";
    for(unsigned int i = 0; i < yPoints.size(); i++){
        //First, the y value
        lagrangeString += "(" + yPoints[i] + ")";
        //Next all the x values
        string numerator = "";
        string denominator = "";
        for(unsigned int j = 0; j < xPoints.size(); j++){
            if(j == i)
                continue;
            numerator += "(x-" + xPoints[j] + ")";
            denominator += "(" + xPoints[i] + "-" + xPoints[j] + ")";
        }
        //Now add the combo to the Lagrange string
        lagrangeString += "((" + numerator + ")/(" + denominator + "))";
        if(i < yPoints.size() - 1)
            lagrangeString += "+";
    }

    //Finally, return the lagrange string
    return lagrangeString;
}