solution_file = "SagePayMvc.sln"
configuration = "release"
test_assemblies = "src/SagePayMvc.Tests/bin/${configuration}/SagePayMvc.Tests.dll"

target default, (compile, test, deploy, package):
	pass

desc "Compiles solution"	
target compile:
	msbuild(file: solution_file, configuration: configuration)

desc "Executes unit tests"
target test:
	nunit(assembly: test_assemblies)

desc "Copies binaries to the build directory"
target deploy:
	rmdir('build')
	
	with FileList():
		.Include("src/SagePayMvc/bin/${configuration}/SagePayMvc.*")
		.Include("License.txt")
		.Include("readme.txt")
		.Flatten(true)
		.ForEach def(file):
			file.CopyToDirectory("build/${configuration}")

desc "Creates zip package"
target package:
	zip("build/${configuration}", "build/SagePayMvc.zip")