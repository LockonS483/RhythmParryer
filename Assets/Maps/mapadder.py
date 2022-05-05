import os

mapname = "tmpin.txt"
increment = 8

def readMap():
    f = open(mapname, 'r')
    lines = f.readlines()
    f.close()

    nf = open("tmpout.txt", "w")
    for l in lines:
        splitstr = l.strip().split(' ')
        newstrarr = (splitstr[0:2])
        newstrarr.append(str(float(splitstr[2]) + increment))
        newstr = ' '.join(newstrarr)
        if(len(splitstr) > 3):
            newstr += ' ' + ' '.join(splitstr[3:])
        nf.write(newstr + '\n')
    nf.close()


if(__name__ == "__main__"):
    readMap()