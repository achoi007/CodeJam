import sys
from bop import BribeOfPrisoners

def main(inFilename, outFilename):
    with open(inFilename) as fin:
        with open(outFilename, "w") as fout:
            numCases = int(fin.readline())
            for i in xrange(1, numCases+1):
                (numCells, numPrisoners) = [int(x) for x in fin.readline().split()]
                prisoners = [int(x) for x in fin.readline().split()]
                bop = BribeOfPrisoners(numCells, prisoners)
                minCost = bop.calcMinCost()
                fout.write("Case #%d: %d\n" % (i, minCost))

if __name__ == '__main__':
    if len(sys.argv) != 3:
        infile = raw_input("Input file:")
        outfile = raw_input("Output file:")
        main(infile, outfile)
    else:
        main(sys.argv[1], sys.argv[2])
    
    
