solution_file = "SagePayMvc.sln"
configuration = "release"
test_assemblies = "src/SagePayMvc.Tests/bin/${configuration}/SagePayMvc.Tests.dll"

target default, (compile, test, deploy, package):
	pass

desc "Compiles solution"	
target compile:
	msbuild(solution_file, { @configuration: configuration })

desc "Executes unit tests"
target test:
	nunit(test_assemblies)

desc "Copies binaries to the build directory"
target deploy:
	rmdir('build')
	mkdir('build')
	mkdir("build\\${configuration}")
	exec("xcopy src\\SagePayMvc\\bin\\${configuration}\\SagePayMvc.dll build\\${configuration}")
	exec("xcopy License.txt build\\${configuration}")
	exec("xcopy readme.txt build\\${configuration}")

desc "Creates zip package"
target package:
	zip("build\\${configuration}", "build\\SagePayMvc.zip")