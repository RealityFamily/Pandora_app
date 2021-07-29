(
	local sAssetsPath = @"{ModelPath}" -- Path to ASSET FOLDER.
	local sFile = (getfiles (sAssetsPath + "*.pan"))[1]
	local IsRendering = false
	local CoronaRenderType = -1
	local CoronaInstalled = classof CoronaRenderer == RendererClass
	local continueMerge = true

	--Checking for active rendering:
	try(IsRendering = maxOps.isRenderActive)catch() -- For Max 2016 and higher.
	try(CoronaRenderType = CoronaRenderer.getRenderType())catch() -- For For Corona 1.6 and higher.
	if CoronaRenderType == -1 then try(IsRendering = CoronaRenderer.isRendering())catch() -- For Corona 1.5.
	
	if IsRendering or (CoronaRenderType == 1) then PushPrompt "Can't merge model during rendering.\nPlease stop render and try again." --Title:"Pandora"
	else (
		if not CoronaInstalled then continueMerge = (queryBox "Corona Render is not installed.\nOther renderers not supported at this moment.\nMerge model?" Title:"Pandora")
		if continueMerge then (
			if sFile == undefined then (pushprompt "Max file not found.") -- Checking max file.
			else (
				if CoronaInstalled then (
					local coronaRenderingType = -1
                   	try (coronaRenderingType = CoronaRenderer.getRenderType())catch() -- Corona 1.5 and lower not support getRenderType method.
					if coronaRenderingType == 2 or coronaRenderingType == 3 then CoronaRenderer.stopRender()
					local aAssetstBefore = #()
					local aAssetsAfter = #()
				)
				rollout HM_MergingDialog "Merging" width:260 height:50 (
					label 'lblText' "Идет перенос. Пожалуйста, подождите." align:#center
					label 'lblText2' "Открываю модель..." align:#center
				)
				::HM_Merging = HM_MergingDialog
				createDialog HM_Merging
				try (macros.run "Asset Tracking System" "AssetTrackingSystemRefresh")catch(ATSOps.Refresh()) -- Empty Array creation.
				ATSOps.GetFiles &aAssetstBefore -- Collects assetes array BEFORE merge.
				
				local currenrTool = toolmode.commandmode			
				mergeMAXFile sFile #select #autoRenameDups #renameMtlDups #neverReparent quiet: true -- MERGE
				if currenrTool != #create then try (toolmode.commandmode = currenrTool)catch()
				::HM_Merging.lblText2.text = "Поиск новых текстур..."
				try (macros.run "Asset Tracking System" "AssetTrackingSystemRefresh")catch(ATSOps.Refresh()) -- Empty Array creation.
				ATSOps.GetFiles &aAssetsAfter -- Collects assetes array AFTER merge.
				if aAssetsAfter.count <= aAssetstBefore.count then (
						pushprompt "No new Assets loaded from merged file"		
					free aAssetstBefore
					free aAssetsAfter
				)
				else (			
					local iFoundAssetsCount = 0
					
					--comparing arrays:
					for i = aAssetsAfter.count to 1 by -1 while iFoundAssetsCount < aAssetstBefore.count do ( --First Cycle start.
						local iCurrentAssetNum = i
						local sCurrentAsset = aAssetsAfter[i]			
						local bMatched = false			
						for asset in aAssetstBefore while bMatched != true do --SubCycle start.
						(
							if asset == sCurrentAsset then
							(					
								iFoundAssetsCount += 1					
								deleteItem aAssetsAfter iCurrentAssetNum
								bMatched = true
							)				
						)--SubCycle end.
					)--First Cycle end.				
					--Warm up ATS:
					if bATSWarmedUp == undefined then (
						if not ATSOps.Visible then (
							ATSOps.Visible = true						
							ATSOps.Visible = false
						)
						global bATSWarmedUp = true
					)
					--Changing textures extension to jpg:
					/*for i = 1 to aAssetsAfter.count do (						
						local type = getFilenameType aAssetsAfter[i]
						type = toLower type
						if type == ".png" or type == ".psd" then (
							local newName = ((getFilenameFile aAssetsAfter[i]) + ".jpg")
							ATSOps.ClearSelection()
							ATSOps.SelectFiles aAssetsAfter[i]						
							aAssetsAfter[i] = newName						
							ATSOps.RetargetSelection newName
						)
					)*/				
					--Retargeting:
					try (
						ATSOps.ClearSelection()  --Clears the selection in the Asset Tracking dialog.		
						ATSOps.SelectFiles &aAssetsAfter --selecting a new assets in the Asset Tracking dialog.			
								
						pushprompt (ATSOps.NumFilesSelected() as string + " new assets loaded from merged file.") -- Displaying how many assets selected.
						
						::HM_Merging.lblText2.text = "Назначение текстур..."        
						
						if not (ATSOps.SetPathOnSelection sAssetsPath CreateOutputFolder:false) then (
							pushprompt ("Error during assets assignment. \nSome textures or IES-files can be missed.\nNew assets count:" + iFoundAssetsCount as string + "\nAssets processed: " + (ATSOps.NumFilesSelected()) as string)
						)
					)catch(pushprompt "Error during assets assignment.")				
				)		
				::HM_Merging.lblText2.text = "Готово. Можно закрыть это окно."
				try (destroyDialog ::HM_Merging) catch(Print "Error")
				-- Resuming interactive rendering:
				if CoronaInstalled then (
					case coronaRenderingType of (
						2:(CoronaRenderer.startInteractiveDocked())
						3:(CoronaRenderer.startInteractive())                            
						--default:()
					)
				)
			)			
		)
	)
)