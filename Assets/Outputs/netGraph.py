import igraph
from argparse import ArgumentParser

def parseargs():
    parser = ArgumentParser()
    parser.add_argument("--file",dest="filename",type=str,default="brain.txt",
        help="file name of the brain dump")
    args = parser.parse_args()
    return args

args = parseargs()

# load data into a graph
g = igraph.Graph.Read_Ncol(args.filename, names=True, weights=True)

for v in g.vs:
	v['size'] = 35
	v['label'] = v['name']
	if v['name'] in ['Pox', 'Poy', 'Age', 'Rnd', 'Alv', 'DFx', 'DFy', 'Fed', 'Stk', 'SFx', 'SFy', 'Spd', 'SAl', 'SHx', 'SHy']:
		v['color'] = 'lightblue'
	elif v['name'] in ['Mvx', 'Mvy']:
		v['color'] = 'lightpink'
	else:
		v['color'] = 'lightgrey'

# convert edge weights to color and size
for e in g.es:
	#print(e['weight'])
	if e['weight'] < 0:
		e['color'] = 'lightcoral'
	elif e['weight'] == 0:
		e['color'] = 'grey'
	else:
		e['color'] = 'green'

	width = abs(e['weight'])
	e['width'] = 1 + 1.25 * (width /1.0)


# plot graph

#print(len(g.vs))

if len(g.vs) < 6:
    bbox = (300,300)
    layout = 'fruchterman_reingold'
elif len(g.vs) < 12:
    bbox = (400,400)
    layout = 'fruchterman_reingold'
elif len(g.vs) < 18:
    bbox = (500,500)
    layout = 'fruchterman_reingold'
elif len(g.vs) < 24:
    bbox = (520,520)
    layout = 'fruchterman_reingold'
elif len(g.vs) < 26:
    bbox = (800,800)
    layout = 'fruchterman_reingold'
elif len(g.vs) < 50:
    bbox = (1000,1000)
    layout = 'fruchterman_reingold'
elif len(g.vs) < 130:
    bbox = (1200,1000)
    layout = 'fruchterman_reingold'
elif len(g.vs) < 150:
    bbox = (4000,4000)
    layout = 'fruchterman_reingold'
    for v in g.vs:
        v['size'] = v['size'] * 1.5
elif len(g.vs) < 200:
    bbox = (4000,4000)
    layout = 'kamada_kawai'
    for v in g.vs:
        v['size'] = v['size'] * 2
else:
    bbox = (8000,8000)
    layout = 'fruchterman_reingold'

igraph.plot(g, "net.svg", edge_curved=True, bbox=bbox, margin=64, layout=layout)