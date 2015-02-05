from __future__ import division
import numpy as np

def solve_for_t(p, v):
    '''
    s = x^2 + y^2 + z^2
    s' = 2xx' + 2yy' + 2zz'
    s'' = 2(x'^2 + y'^2 + z'^2) - always positive means local minimum

    Critical point when s' = 0:
    xx' + yy' + zz' = 0

    since
    x = x0 + tx'
    y = y0 + ty'
    z = z0 + tz'

    equation simplifies to:
    t((x')^2 + (y')^2 + (z')^2) + x'x0 + y'y0 + z'z0 = 0

           x'x0 + y'y0 + z'z0
    t = -  ------------------------
           (x')^2 + (y')^2 + (z')^2
    '''
    psum = np.sum(p, axis=0)
    vsum = np.sum(v, axis=0)
    numerator = -np.dot(psum, vsum)
    denominator = np.dot(vsum, vsum)
    return numerator / denominator

def solve_for_d(p, v, t):
    pfinal = p + v * t
    cm = np.average(pfinal, axis=0)
    return np.linalg.norm(cm)

def read_next(f):
    n = int(f.readline())
    pos = np.empty((n, 3))
    vel = np.empty((n, 3))
    for i in xrange(n):
        line = f.readline().split()
        pos[i, :] = [int(line[n]) for n in range(3)]
        vel[i, :] = [int(line[n]) for n in range(3, 6)]
    return (pos, vel)

def solve(f):
    n = int(f.readline())
    for i in range(n):
        (pos, vel) = read_next(f)
        t = solve_for_t(pos, vel)
        d = solve_for_d(pos, vel, t)
        print "Case #{0}: {1}".format(i+1, d, t)




    


