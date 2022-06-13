import sys

def bunnyHops(n):
    # Base Cases
    if n == 0:
        return 0
    if n == 1:
        return 1
    if n == 2:
        return 2
    if n == 3:
        return 4
    
    # Starting at n == 4, these are the last three values
    # This list will only ever contain the last three values computed,
    # meaning it will function as a queue
    lastThree = [4, 2, 1]
    i = 3
    # Until we've calculated all the values up to n...
    while i < n:
        sum = lastThree[0] + lastThree[1] + lastThree[2]
        # remove lastThree[2] (the oldest value)...
        lastThree.pop()
        # ... and add the new value!
        lastThree.insert(0, sum)
        i += 1
    return lastThree[0]

if __name__ == "__main__":
    correct_usage = "\n\tCorrect usage:\n\t\tpython BunnyHopper.py N\n\t\t(Where N is an int representing the distance to check)"
    if(len(sys.argv) < 2):
        print("Error: You need to pass in a value." + correct_usage)
    elif(len(sys.argv) > 2):
        print("Error: Too many arguments." + correct_usage)
    else:
        try:
            sum = bunnyHops(int(sys.argv[1]))
            print(sum)
        except ValueError:
            print("Error: Cannot parse `" + sys.argv[1] + "` into an integer" + correct_usage)