inputFile="Thesis (new).tex"
outFile="Thesis (new).pdf"
outDir=Build

entry:
	latexmk \
		-interaction=nonstopmode -f \
		-quiet \
		-pdf \
		-outdir=$(outDir) \
		$(inputFile)

clean:
	cd $(outDir) && latexmk -c $(outFile)