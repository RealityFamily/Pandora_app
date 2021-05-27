(
	--Getting button status:
	local INIpath = ("$userScripts\\3dhamster\\3dhamster.ini")
	if (getINISetting INIpath "UI" "ButtonStatus") != "installed" then (

		local ScriptFolder = getFilenamePath (getSourceFileName())
		local HamsterMacroScriptName = "\3D Hamster-Launch3DHamster.mcr"
		local HamsterMacroScriptPath =  (ScriptFolder + HamsterMacroScriptName)
		
		if not doesFileExist HamsterMacroScriptPath then print "Не найден исходный макроскрипт."
		else (
			--///// MacroScript installation: /////
			local targetMacroScriptPath = ((GetDir #userMacros) + HamsterMacroScriptName)
			
			deleteFile targetMacroScriptPath
			copyFile HamsterMacroScriptPath targetMacroScriptPath
			try (fileIn targetMacroScriptPath)catch(print "Не удалось запустить макроскрипт.")
			
			--///// Icon copying: /////			
			local SourceIconsPath = (ScriptFolder + @"\icons")
			
			local smalIconName = "\3DHamster_16i.bmp"
			local iconSmallPath = (SourceIconsPath + smalIconName)
			local targetSmallIconFile  = ((GetDir #userIcons) + smalIconName)			
			copyFile iconSmallPath targetSmallIconFile			
			
			local BigIconName = "\3DHamster_24i.bmp"
			local iconBigPath = (SourceIconsPath + BigIconName)
			local targetBigIconFile  = ((GetDir #userIcons) + BigIconName)			
			copyFile iconBigPath targetBigIconFile
			
			try (colorman.reInitIcons())catch() -- UI Refresh.
			
			--///// Toolbar modification: /////
			(
				local ButtonCode = "<Item typeID=\"2\" type=\"CTB_MACROBUTTON\" width=\"0\" height=\"0\" controlID=\"0\" macroTypeID=\"3\" macroType=\"MB_TYPE_ACTION\" actionTableID=\"647394\" imageID=\"-1\" imageName=\"\" actionID=\"Launch3DHamster`3D Hamster\" tip=\"Launch 3D Hamster\" label=\"3D Hamster\" />"				
				local ExistingButton = "Launch3DHamster`3D Hamster"				
				--colorman.reInitIcons()
				fn addIconToMainToolBar ButtonCode ExistingButton = (
					local ButtonCreated = false
					local sToReplace = "</Items>"
					--Getting path to tollbars config file:
					Local fileToolbarCFG = cui.getConfigFile()					
					--Load UI from file:
					cui.loadConfig fileToolbarCFG
					cui.saveConfigAs fileToolbarCFG
					cui.loadConfig fileToolbarCFG
					--Getting File Size:
					local tempString = MemStreamMgr.openFile fileToolbarCFG
					local FileSize = tempString.size()
					MemStreamMgr.close tempString
					--Loading tollbar config file as stream:    
					local stream = openFile fileToolbarCFG mode:"r+"
					try(
						-- Setting the cursor to zero position:
						seek stream 0
						-- Setting the cursor to start of the Main Toolbar section:            
						skipToString stream "\"Main Toolbar\""
						local iMainToolbarBegin = filePos stream
						
						skipToString stream sToReplace
						local iMainToolbarEnd = filePos stream
						seek stream iMainToolbarBegin
						local bButtonExist = (skipToString stream ExistingButton)
						local existingButtonCodePos = filePos stream
						if bButtonExist == undefined or (existingButtonCodePos < iMainToolbarBegin or existingButtonCodePos > iMainToolbarEnd) then (
							--UI BackUp creation:
							backUpFilePath = ((getFilenamePath  fileToolbarCFG) + (getFilenameFile   fileToolbarCFG) + "BackUp.cuix" )
							deleteFile backUpFilePath
							copyFile fileToolbarCFG backUpFilePath
							seek stream iMainToolbarEnd

							-- Getting insertion position:
							local pos = filePos stream
							local iInsertiontPosition = (pos - sToReplace.count)
							seek stream iInsertiontPosition
							
							-- Getting text fragment:
							local iFragmentLength = FileSize - iInsertiontPosition
							local sContentAfterInsert = readChars stream iFragmentLength
							
							-- Insertring new ButtonCode:
							seek stream iInsertiontPosition
							format ButtonCode to: stream

							-- Inserting old buttons code back:
							seek stream (iInsertiontPosition + ButtonCode.count)
							format sContentAfterInsert to: stream

							--Closing stream:
							close stream
							ButtonCreated = true

							--UI Refreshing:
							cui.loadConfig fileToolbarCFG
							cui.saveConfigAs fileToolbarCFG
						)
						else (
							close stream
						)
					)
					catch(
						close stream
						print "Ошибка в процессе добавления кнопки."
					)
					return ButtonCreated
				)
				
				--Button creation:
				local ButtonCreated = (addIconToMainToolBar ButtonCode ExistingButton)
				--Setting button status:
				setINISetting INIpath "UI" "ButtonStatus" "installed"
			)
			
			if ButtonCreated == true then (
				try (DestroyDialog ::DlgInstallCompleted)catch()

				::DlgInstallCompleted = rollout rollCompleted "3D Hamster" width:280 (	
					local iconBitmap = openBitMap "$userIcons\\3DHamster_24i.bmp"
					group ""(
						label 'lblInfo1' "3D Hamster успешно установлен." align:#center
						label 'lblInfo2' "Вы можете найти его на панели инструментов:" align:#center
						imgtag 'HMicon' "imgtag" bitmap:iconBitmap align:#center
						label 'lblInfo3' "Или в категории \"3D Hamster\"." align:#center
						label 'lblInfo4' "Также вы можете запустить 3D Hamster" align:#center
						label 'lblInfo5' "через ярлык на рабочем столе." align:#center
					)
					
					button 'btnClose' "Ok" align:#center width:180 height:32
					
					on 'btnClose' pressed do (
						try (destroyDialog ::DlgInstallCompleted) catch()
					)
				)
				createDialog ::DlgInstallCompleted pos:[(systemTools.GetScreenWidth()/2 - 90),(systemTools.GetScreenHeight()/2 - 120)]
			)
		)
	)
)