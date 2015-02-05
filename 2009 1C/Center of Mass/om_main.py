import sys
import cm

if len(sys.argv) != 3:
    print "Usage:", sys.argv[0], "infile outfile"
    sys.exit(1)

infile = open(sys.argv[1], 'r')
solns = cm.solve(infile)
infile.close()

for (i,v) in enumerate(solns):
    pass

