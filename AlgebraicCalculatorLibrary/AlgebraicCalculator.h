#ifndef ALGEBRAICCALCULATOR_H
#define ALGEBRAICCALCULATOR_H

/*
This class is an algebraic calculator (which works exclusively with the 'x' character).
It takes in a given function string in its constructor (e.g. "x^2+3x+4"), and can then
call f_of(some double) to calculate the answer (for our function we gave in the example
above, it would be f_of_(2) = (2)^2+3(2)+4).

This class relies heavily on the Mathematical Expression Toolkit Library, created by
Arash Partow. Learn more at http://www.partow.net/programming/exprtk/index.html
*/
#include <iostream>

using namespace std;

class AlgebraicCalculator {
public:
    AlgebraicCalculator();
    AlgebraicCalculator(string givenStr);

    double f(double val);

private:
    //The given string to calculate
    string givenString;
};

#endif // ALGEBRAICCALCULATOR_H

