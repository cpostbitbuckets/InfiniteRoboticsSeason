-- export topdown with 1.4 camera distance
intakePath = "Primitives/cylinder"
shooterPath = "Blasters/clipBarrel"

function showAll ()
    forge.print("showing all")
    forge.selectNone()
    forge.invertSelection()
    forge.showSelection()        
end

function hideIntake ()
    forge.print("hiding intake")
    forge.selectAll(intakePath)
    forge.hideSelection()
end

function hideShooter ()
    shooter = forge.selectAll(shooterPath)
    forge.hideSelection()
end

function exportDrivebases ()
    hideIntake()
    hideShooter()
    forge.print("exporting drivebase")
    forge.exportSprite(512, "/tmp/drivebase01")    
end

function exportIntakes ()
    -- export all intakes, currently only have one 

    -- Select the cylinder, invert it, export it
    forge.selectAll(intakePath)
    forge.invertSelection()
    forge.hideSelection()
    
    -- now save just the intake
    forge.print("exporting intake")
    forge.exportSprite(512, "/tmp/intake01")    
end

function exportShooters ()
    -- hide everything but the intake
    forge.selectAll(shooterPath)
    forge.invertSelection()
    forge.hideSelection()
    forge.print("exporting shooter")
    forge.exportSprite(512, "/tmp/shooter01")

end

-- start with everything visible
showAll()

forge.print("exporting collection")
forge.exportSprite(512, "/tmp/robot01")

-- Export just the drivebase
exportDrivebases()

-- export the intakes
showAll()
exportIntakes()

showAll()
exportShooters()

showAll()
