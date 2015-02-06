import sys
import cm

def solve_file(inpath, outpath):

    # Input file
    infile = open(inpath, 'r')
    solns = cm.solve(infile)

    # Output
    outfile = open(outpath, 'w')
    for (i,v) in enumerate(solns):
        outfile.write("Case #{0}: {1:.8f} {2:.8f}\n".format(i+1, v[1], v[0]))
    outfile.close()
    infile.close()

if __name__ == '__main__':
    if len(sys.argv) == 3:
        infile = sys.argv[1]
        outfile = sys.argv[2]        
    else:
        infile = raw_input("input pathname:")
        outfile = raw_input("output pathname:")
    solve_file(infile, outfile)

        

                  
                  


