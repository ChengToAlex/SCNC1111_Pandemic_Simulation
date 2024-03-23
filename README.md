# SCNC1111_Pandemic_Simulation
SCNC1111 is a year 1 course offered by the Faculty of Science HKU, and this is a program I wrote in C# .NET Framework 4.8

1. The program will create a list of people in a population with 3 characteristics assigned to each person, ID, whether this person is infected and apprehended by the government.
2. In each cycle, an infected person will contact a random number of healthy individuals (the maximum number (S) can be set), and they have a chance (Pi Probability of infection) to infect these healthy people.
3. In each cycle, an infected person will have a chance to be cured (Pc Probability of cure) or arrested/quarantined (Pa Probability of arrest)
4. Contagious populations are those who are infected but not arrested/quarantined

This simulation assumes that the disease is curable (% of people to be cured in 1 cycle which can be days, weeks or years, e.g. Tamiru et al. (2023): COVID recovery day median is 9 days, 50% of patients recover within 9 days) and it is not lethal (no decrease in population size)

The Number of Infected population and Contagious population will be monitored and the result will be exported into an Excel file in the Output folder, using the ClosedXML packages (package not included in the source code). 
