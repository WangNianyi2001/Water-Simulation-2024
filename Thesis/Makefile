inputFile=document.tex
outFile=document.pdf
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