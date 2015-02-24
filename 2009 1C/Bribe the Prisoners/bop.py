import sys
import unittest

class BribeOfPrisoners(object):

    def __init__(self, numCells, prisoners):
        self.numCells = numCells
        # Prisoners are numbered from 1 to N, code works better for range from 0 to N-1
        self.prisoners = [p-1 for p in prisoners]

    def calcMinCost(self):
        costDict = {}
        return self.calcMinCostDynamic(0, self.numCells,
                                       0, len(self.prisoners),
                                       self.prisoners,
                                       costDict)

    def calcMinCostDynamic(self, cellStart, cellEnd, prisonStart, prisonEnd,
                           prisoners, costDict):
        # Done if no prisoner is left
        if prisonStart >= prisonEnd:
            return 0
        # If cost is previously computed, just use it
        key = (cellStart, cellEnd)
        if key in costDict:
            return costDict[key]
        # The cost of choosing i-th prisoner this round is sum of:
        # * moving the i-th prisoner
        # * moving all prisoners to left of i-th prisoner
        # * moving all prisoners to right of i-th prisoner
        minCost = sys.maxint
        for i in xrange(prisonStart, prisonEnd):
            prisoner = prisoners[i]
            cost = (prisoner - cellStart) + (cellEnd - prisoner - 1)
            if cost >= minCost:
                continue
            # Prisoners to the left
            cost += self.calcMinCostDynamic(cellStart, prisoner,
                                            prisonStart, i,
                                            prisoners, costDict)
            if cost >= minCost:
                continue
            # Prisoners to the right
            cost += self.calcMinCostDynamic(prisoner + 1, cellEnd,
                                            i + 1, prisonEnd,
                                            prisoners, costDict)
            if cost < minCost:
                minCost = cost
        costDict[key] = minCost
        return minCost

class BribeOfPrisonersTest(unittest.TestCase):

    def test_1(self):
        bop = BribeOfPrisoners(8, [3])
        self.assertEqual(7, bop.calcMinCost())

    def test_2(self):
        bop = BribeOfPrisoners(20, [3, 6, 14])
        self.assertEqual(35, bop.calcMinCost())
        
if __name__ == '__main__':
    unittest.main()
