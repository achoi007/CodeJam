from __future__ import division
import numpy as np
import unittest
import StringIO

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
    # denominator is zero means cm will never move
    if denominator == 0:
        return 0
    t = numerator / denominator
    # t < 0 means critical point is before start.
    if t < 0:
        return 0
    else:
        return t

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

def solve_next(f):
    (pos, vel) = read_next(f)
    t = solve_for_t(pos, vel)
    d = solve_for_d(pos, vel, t)
    return (t, d)

def solve(f):
    n = int(f.readline())
    for i in xrange(n):
        yield solve_next(f)

class CMTest(unittest.TestCase):

    def setUp(self):
        self.ep = 4
        self.case1 = self.create_case(["3 0 -4 0 0 3",
                                       "-3 -2 -1 3 0 0",
                                       "-3 -1 2 0 3 0"])
        self.case2 = self.create_case(["-5 0 0 1 0 0",
                                       "-7 0 0 1 0 0",
                                       "-6 3 0 1 0 0"])
        self.case3 = self.create_case(["1 2 3 1 2 3",
                                       "3 2 1 3 2 1",
                                       "1 0 0 0 0 -1",
                                       "0 10 0 0 -10 -1"])

    def create_case(self, lines):
        s = str(len(lines)) + "\n"
        s += "\n".join(lines)
        return StringIO.StringIO(s)

    def test_read_next(self):
        (pos, vel) = read_next(self.case1)
        self.assertTrue((pos == np.array([[3, 0, -4],
                                          [-3, -2, -1],
                                          [-3, -1, 2]])).all())
        self.assertTrue((vel == np.array([[0, 0, 3],
                                          [3, 0, 0],
                                          [0, 3, 0]])).all())

    def test_solve(self):
        (t, d) = solve_next(self.case1)
        self.assertAlmostEqual(t, 1, self.ep)
        self.assertAlmostEqual(d, 0, self.ep)
        (t, d) = solve_next(self.case2)
        self.assertAlmostEqual(t, 6, self.ep)
        self.assertAlmostEqual(d, 1, self.ep)
        (t, d) = solve_next(self.case3)
        self.assertAlmostEqual(t, 1, self.ep)
        self.assertAlmostEqual(d, 3.3634, self.ep)        


if __name__ == '__main__':
    unittest.main()




    


