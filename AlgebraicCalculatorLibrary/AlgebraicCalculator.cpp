#include "AlgebraicCalculator.h"
#include "expertk.hpp"

#include <string>

using namespace std;

/*
This function is used by f_of_(double val) to calculate our answer
(This function uses the Mathematical Expression Toolkit Library).
Sets calculatedAnswer variable to the answer.
*/
double METL_Expression_Calculation(string algebraExpression) {
    //Ideas for this function taken from:
    //https://stackoverflow.com/questions/5115872/what-is-the-best-way-to-evaluate-mathematical-expressions-in-c
    typedef exprtk::expression<double> expression_t;
    typedef exprtk::parser<double> parser_t;

    string expression_string = algebraExpression;

    expression_t expression;

    parser_t parser;
    parser.compile(expression_string, expression);

    return expression.value();
}


/*
The default constructor (really will never be used)
*/
AlgebraicCalculator::AlgebraicCalculator() {
    givenString = "x";
}

/*
Constructor that takes in an algebraic string that we want calculated
*/
AlgebraicCalculator::AlgebraicCalculator(string givenStr) {
    givenString = givenStr;
}

/*
This function calculates the string we gave in the constructor as the function
with the value we passed in here as the x value, and returns the calculated value
(calculated using the function METL_Expression_Calculation).
*/
double AlgebraicCalculator::f(double val) {
    string stringValReplacement = "(" + to_string(val) + ")";

    string algebraicString = "";
    //Replace all x values with given val
    for (unsigned int i = 0; i < givenString.length(); i++) {
        if (givenString.at(i) == 'x')
            algebraicString += stringValReplacement;
        else
            algebraicString += givenString.at(i);
    }

    //Use the Mathematical Expression Toolkit Library to calculate our answer
    return METL_Expression_Calculation(algebraicString);
}



