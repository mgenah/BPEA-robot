JOB_OUT="$(/opt/sge/bin/lx-amd64/qsub -l h_vmem=8G -q sipper.q -cwd runScript.sh -sync y)"
ARR=($JOB_OUT)
FILE=runScript.sh.o"${ARR[2]}"
while [ ! -f "$FILE" ]; do
	sleep 1;
done

SIZE=0
while [ "$SIZE" = 0 ]; do
	SIZE="$(wc -c "$FILE" | awk '{print $1}')"
	sleep 1;
done

cat "$FILE"
