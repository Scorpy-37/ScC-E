str=""
for f in *; do
	if [ "$f" != "compile.sh" ] && [ "$f" != "Game.exe" ]; then
		str="$str $f"
	fi
done
echo "Compiling scripts: $str"

echo "../../dotnet/csc.exe /out:Game.exe ..\\Program.cs$str"
../../dotnet/csc.exe /out:Game.exe ..\\Program.cs$str

echo ""
read -n 1 -p "Compiled, click enter to run game"

./Game.exe
echo ""
read -n 1 -p "Runtime finished"