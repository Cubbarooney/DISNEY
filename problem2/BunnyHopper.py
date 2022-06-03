def bunnyHops(n):
    hops = bunnyHopsHelper([], n)
    print("n=" + str(n))
    print("len=" + str(len(hops)))
    for h in hops:
        print(h)

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
    bunnyHops(1)
    bunnyHops(2)
    bunnyHops(3)
    bunnyHops(4)
    bunnyHops(5)
    bunnyHops(6)