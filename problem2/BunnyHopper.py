import sys

# Given n, find how many unique combinations of hops the bunny can take
# This method uses recursion to slowly build out all the possible answers
def bunnyHops(n):
    hops = bunnyHopsHelper([], n)
    return len(hops)

def bunnyHopsHelper(hops, n):
    # Base Case: Finished
    #   (n is zero, meaning the bunny traveled the entire distance!)
    if n == 0:
        return [hops]
    else:
        # list of possible answers
        ret_hops = []
        # Try adding either 1, 2, or 3 hops to the current list of hops
        for i in range(1, 4):
            # But only if we can still add i
            if n - i >= 0:
                new_hops = hops.copy()
                new_hops.append(i)
                result = bunnyHopsHelper(new_hops, n - i)
                # add results to our list of possible answers
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
            print("Recursive Method:")
            sum = bunnyHops(int(sys.argv[1]))
            print(sum)
        except ValueError:
            print("Error: Cannot parse `" + sys.argv[1] + "` into an integer" + correct_usage)