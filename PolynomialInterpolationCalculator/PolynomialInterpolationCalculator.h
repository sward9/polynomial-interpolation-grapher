#include <iostream>

using namespace std;

/*
This class takes in a set of (x,y) points as a csv string, and creates
a Lagrange interpolation polynomial that goes through all those points.
Gives the interpolation polynomial in the form of a string.
*/
class PolynomialInterpolationCalculator{
public:
    PolynomialInterpolationCalculator();
    PolynomialInterpolationCalculator(string csvPts);

    string getLagrangePolynomial();
private:
    string csvPoints;
};