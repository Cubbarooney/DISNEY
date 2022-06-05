import sys
import itertools
import time

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

# given a list, finds all the *unique* permutations
def permutate(hop):
    perm = itertools.permutations(hop)
    # Because itertools.permutations determines uniqueness based on index, not value
    # we need to first convert to a set and then into a list. This removes duplciate
    # entries.
    return list(set(perm))

# This version is far more complicated and less readible
# But the basic idea is we get the answer that has the least hops possible
# i.e. the most 3-hops
# We can then generate new lists based on the following properties:
#   - [3] is the same as [1,1,1], [1,2]
#   - [2] is the same as [1,1]
#   - [3,3] is the same as [2,2,2]
#   - [3,1] is the same as [2,2] (only needed at the begining)
# Using these rules, we can then generate a new list
# Pros: No recursion!
# Cons: MUCH slower, and far less readible
#           At n = 12, it took 81.5 seconds. The Recursive method took 0.0026 seconds
#       Thus, this is not my prefered method
#       Also, I've only tested this method up through n=12
def bunnyHops_ALT(n):
    # Base Cases (saves a lot of time (conceptually))
    if n == 3:
        return 4
    if n == 2:
        return 2
    if n == 1:
        return 1
    if n == 0:
        return 0
    
    # stack represents valid answers that we can iterate off of
    stack = []
    shortest_path = []
    # Create the shortest path and add it to the stack
    for i in range(n // 3):
        shortest_path.append(3)
    if n % 3 == 2:
        shortest_path.append(2)
    elif n % 3 == 1:
        # because [3,1] is the same as [2,2], we need to generate that answer now
        temp = shortest_path.copy()
        temp.pop(len(temp) - 1)
        temp.extend([2,2])
        stack.append(temp)
        # now we can add 1
        shortest_path.append(1)
    stack.append(shortest_path.copy())

    # handle the cases where we have [3,3] in the list and replace it with [2,2]
    temp_stack = []
    # We need to check all answers currently in the stack
    for s in stack:
        temp = s.copy()
        i = 0
        # iterate over the array, looking at values as pairs
        while i < len(temp) - 1:
            if temp[i] == 3 and temp[i+1] == 3:
                temp.pop(i)
                temp.pop(i)
                # add the [2,2,2] to the end of the list
                temp.extend([2,2,2])
                temp_stack.append(temp.copy())
            else:
                # if we end up here, then we know that there are no more 3
                # By nature of how we did the creation of the shortest path,
                # we know the list is sorted High to Low
                break
    # merge the stacks
    stack.extend(temp_stack)
    # hops keeps track of all the *unique* answers
    hops = []
    # while the stack isn't empty
    while len(stack) > 0:
        curr = stack.pop(0)
        # if this answer is already in hops, then we have seen it before.
        # No need to process it again
        if curr in hops:
            continue
        # Get all the *unique* permutations for this answer
        perms = permutate(curr)
        for p in perms:
            # p is by default a tuple, so convert it to a list
            p = list(p)
            # Just in case, if we already have this answer, skip it
            if p in hops:
                continue
            # We found a new answer!
            hops.append(p)
            # Now it is time to generate more...
            pcopy = p.copy()
            # If we have a 2 or 3, then we need to try replacing them
            # with their respectives rules
            if pcopy[0] == 2:
                # Replace the [2] with [1,1]
                pcopy.pop(0)
                temp = [1,1]
                temp.extend(pcopy)
                stack.append(temp)
            elif pcopy[0] == 3:
                # Replace the [3] with [1,1,1]
                pcopy.pop(0)
                temp = [1,1,1]
                temp.extend(pcopy)
                stack.append(temp)
                # Replace the [3] with [1,2]
                temp = [1,2]
                temp.extend(pcopy)
                stack.append(temp)
    # finally, return the length of hops!
    return len(hops)

if __name__ == "__main__":
    correct_usage = "\n\tCorrect usage:\n\t\tpython BunnyHopper.py N\n\t\t(Where N is an int representing the distance to check)"
    if(len(sys.argv) < 2):
        print("Error: You need to pass in a value." + correct_usage)
    elif(len(sys.argv) > 2):
        print("Error: Too many arguments." + correct_usage)
    else:
        try:
            print("Recursive Method:")
            tic = time.perf_counter()
            sum = bunnyHops(int(sys.argv[1]))
            toc = time.perf_counter()
            print(sum)
            print(f"(Took {toc - tic:0.4f} seconds)")
            print("Looping Method:")
            tic = time.perf_counter()
            sum = bunnyHops_ALT(int(sys.argv[1]))
            toc = time.perf_counter()
            print(sum)
            print(f"(Took {toc - tic:0.4f} seconds)")
        except ValueError:
            print("Error: Cannot parse `" + sys.argv[1] + "` into an integer" + correct_usage)