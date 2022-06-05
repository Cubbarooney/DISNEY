import sys

def bunnyHops(n):
    hops = bunnyHopsHelper([], n)
    # print("n=" + str(n))
    # print("len=" + str(len(hops)))
    # for h in hops:
    #     print(h)
    return len(hops)

def bunnyHopsHelper(hops, n):
    # Base Case: Finished
    #   (n is zero, meaning the bunny traveled the entire distance!)
    if n == 0:
        return [hops]
    else:
        ret_hops = []
        for i in range(1, 4):
            if n - i >= 0:
                new_hops = hops.copy()
                # Try adding either 1, 2, or 3 hops to the current list of hops
                new_hops.append(i)
                result = bunnyHopsHelper(new_hops, n - i)
                if result != None:
                    ret_hops.extend(result)
        return ret_hops


if __name__ == "__main__":
    correct_usage = "\n\tCorrect usage:\n\t\tpython BunnyHopper.py N\n\t\t(Where N is an int representing the distance to check)"
    if(len(sys.argv) < 2):
        print("Error: You need to pass in a value." + correct_usage)
    elif(len(sys.argv) > 2):
        print("Error: Too many arguments." + correct_usage)
    else:
        try:
            print(bunnyHops(int(sys.argv[1])))
        except ValueError:
            print("Error: Cannot parse `" + sys.argv[1] + "` into an integer" + correct_usage)