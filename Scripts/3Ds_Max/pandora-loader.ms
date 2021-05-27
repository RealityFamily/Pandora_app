(	
	local PandoraRootFolder
	try(
		local key
		registry.openKey HKEY_CURRENT_USER ("Software\\Pandora") accessRights:#readOnly key:&key
		registry.queryValue key "InstallPath" value:&PandoraRootFolder expand:true	
		registry.closeKey key
	)catch()
	
	if PandoraRootFolder != undefined then (
		local PandoraScript =  (PandoraRootFolder + "\Scripts\pandora.ms")
		if doesFileExist PandoraScript then filein PandoraScript
	)
)